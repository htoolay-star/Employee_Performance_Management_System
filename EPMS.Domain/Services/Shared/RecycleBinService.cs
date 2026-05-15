using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Hr;
using EPMS.Domain.Entities.Performance;
using EPMS.Domain.Entities.Shared;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.RecycleBin;
using EPMS.Shared.Enums;

namespace EPMS.Domain.Services.Shared
{
    public interface IRecycleBinService
    {
        Task<SuccessResponse<IEnumerable<RecycleBinItemDto>>> GetAllDeletedAsync();
        Task<SuccessResponse> RestoreAsync(string entityType, long entityId);
    }

    public class RecycleBinService : IRecycleBinService
    {
        private readonly IUnitOfWork _uow;
        private readonly Dictionary<string, Func<long, Task<SuccessResponse>>> _restoreMap;

        public RecycleBinService(IUnitOfWork uow)
        {
            _uow = uow;
            _restoreMap = new Dictionary<string, Func<long, Task<SuccessResponse>>>
            {
                ["APPRAISALCYCLE"] = id => RestoreViaService(_uow.Perf.AppraisalCycles, id),
                ["RATINGSCALE"] = id => RestoreViaService(_uow.Perf.RatingScales, id),
                ["KPIWEIGHTPRIORITY"] = id => RestoreViaService(_uow.Perf.KPIWeightPriorities, id),
                ["QUESTIONRATINGSCALE"] = id => RestoreViaService(_uow.Perf.QuestionRatingScales, id),
                ["LEVEL"] = id => RestoreViaService(_uow.HR.Levels, id),
                ["DEPARTMENT"] = id => RestoreViaService(_uow.HR.Departments, id),
                ["POSITION"] = id => RestoreViaService(_uow.HR.Positions, id),
                ["TEAM"] = id => RestoreViaService(_uow.HR.Teams, id),
                ["CATEGORY"] = id => RestoreViaService(_uow.Shared.Categories, id),
            };
        }

        public async Task<SuccessResponse<IEnumerable<RecycleBinItemDto>>> GetAllDeletedAsync()
        {
            var items = new List<RecycleBinItemDto>();

            await AddDeletedAsync(items, "APPRAISALCYCLE", _uow.Perf.AppraisalCycles, e => ((AppraisalCycle)e).Name);
            await AddDeletedAsync(items, "RATINGSCALE", _uow.Perf.RatingScales, e => ((RatingScale)e).Label);
            await AddDeletedAsync(items, "KPIWEIGHTPRIORITY", _uow.Perf.KPIWeightPriorities, e => ((KPIWeightPriority)e).LevelName);
            await AddDeletedAsync(items, "QUESTIONRATINGSCALE", _uow.Perf.QuestionRatingScales, e => ((QuestionRatingScale)e).Name);
            await AddDeletedAsync(items, "LEVEL", _uow.HR.Levels, e => ((Level)e).Name);
            await AddDeletedAsync(items, "DEPARTMENT", _uow.HR.Departments, e => ((Department)e).Name);
            await AddDeletedAsync(items, "POSITION", _uow.HR.Positions, e => ((Position)e).Name);
            await AddDeletedAsync(items, "TEAM", _uow.HR.Teams, e => ((Team)e).Name);
            await AddDeletedAsync(items, "CATEGORY", _uow.Shared.Categories, e => ((Category)e).Name);

            return SuccessResponse<IEnumerable<RecycleBinItemDto>>.Ok(
                items.OrderByDescending(x => x.DeletedAt), "Retrieved");
        }

        public async Task<SuccessResponse> RestoreAsync(string entityType, long entityId)
        {
            var upper = entityType.ToUpperInvariant();
            if (!_restoreMap.TryGetValue(upper, out var serviceCall))
                return SuccessResponse.Fail("Unknown entity type.", ErrorType.Validation);

            return await serviceCall(entityId);
        }

        private async Task AddDeletedAsync<T>(List<RecycleBinItemDto> items, string entityType,
            IGenericRepository<T> repo, Func<T, string> getName) where T : class
        {
            var deleted = await repo.GetAllDeletedAsync();
            foreach (var e in deleted)
            {
                var sd = e as ISoftDeletable;
                items.Add(new RecycleBinItemDto
                {
                    EntityType = entityType,
                    EntityId = GetId(e),
                    DisplayName = getName(e),
                    DeletedAt = sd?.DeletedAt ?? DateTimeOffset.UtcNow,
                    DeletedBy = sd?.DeletedBy
                });
            }
        }

        private static long GetId<T>(T e)
        {
            var prop = typeof(T).GetProperty("Id");
            return prop != null ? (long)prop.GetValue(e)! : 0;
        }

        private async Task<SuccessResponse> RestoreViaService<T>(IGenericRepository<T> repo, long id)
            where T : class, ISoftDeletable
        {
            var entity = await repo.GetByIdAsync(id);
            if (entity == null)
                return SuccessResponse.Fail("Item not found.", ErrorType.NotFound);
            if (!entity.IsDeleted)
                return SuccessResponse.Fail("Item is not deleted.", ErrorType.Validation);

            entity.IsDeleted = false;
            entity.DeletedAt = null;
            entity.DeletedBy = null;
            repo.Update(entity);
            await _uow.CompleteAsync();
            return SuccessResponse.Ok("Item restored successfully.");
        }
    }
}
