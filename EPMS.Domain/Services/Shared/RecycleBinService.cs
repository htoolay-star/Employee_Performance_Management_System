using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Hr;
using EPMS.Domain.Entities.Performance;
using EPMS.Domain.Entities.Shared;
using EPMS.Domain.Interface.IService.App;
using EPMS.Domain.Interface.IService.Hr;
using EPMS.Domain.Interface.IService.Performance;
using EPMS.Domain.Interface.IService.Shared;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.RecycleBin;
using EPMS.Shared.Enums;
using System.Linq;

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
        private readonly ICurrentEmployeeContextService _currentEmployee;
        private readonly Dictionary<string, Func<long, Task<SuccessResponse>>> _restoreMap;

        public RecycleBinService(
            IUnitOfWork uow,
            ICurrentEmployeeContextService currentEmployee,
            ILevelService levelService,
            IDepartmentService departmentService,
            ITeamService teamService,
            IPositionService positionService,
            IAppraisalCycleService appraisalCycleService,
            IRatingScaleService ratingScaleService,
            IKPIWeightPriorityService kpiWeightPriorityService,
            IQuestionRatingScaleService questionRatingScaleService,
            ICategoryService categoryService)
        {
            _uow = uow;
            _currentEmployee = currentEmployee;
            _restoreMap = new Dictionary<string, Func<long, Task<SuccessResponse>>>
            {
                ["APPRAISALCYCLE"] = id => appraisalCycleService.RestoreAsync(id),
                ["RATINGSCALE"] = id => ratingScaleService.RestoreAsync(id),
                ["KPIWEIGHTPRIORITY"] = id => kpiWeightPriorityService.RestoreAsync(id),
                ["QUESTIONRATINGSCALE"] = id => questionRatingScaleService.RestoreAsync(id),
                ["LEVEL"] = id => levelService.RestoreAsync(id),
                ["DEPARTMENT"] = id => departmentService.RestoreAsync(id),
                ["POSITION"] = id => positionService.RestoreAsync(id),
                ["TEAM"] = id => teamService.RestoreAsync(id),
                ["CATEGORY"] = id => categoryService.RestoreCategoryAsync(id),
            };
        }

        public async Task<SuccessResponse<IEnumerable<RecycleBinItemDto>>> GetAllDeletedAsync()
        {
            var currentEmployeeId = await _currentEmployee.GetEmployeeIdAsync();
            if (!currentEmployeeId.HasValue)
                return SuccessResponse<IEnumerable<RecycleBinItemDto>>.Ok(
                    Enumerable.Empty<RecycleBinItemDto>(), "Retrieved");

            var items = new List<RecycleBinItemDto>();

            await AddDeletedAsync(items, "APPRAISALCYCLE", _uow.Perf.AppraisalCycles, e => ((AppraisalCycle)e).Name, currentEmployeeId.Value);
            await AddDeletedAsync(items, "RATINGSCALE", _uow.Perf.RatingScales, e => ((RatingScale)e).Label, currentEmployeeId.Value);
            await AddDeletedAsync(items, "KPIWEIGHTPRIORITY", _uow.Perf.KPIWeightPriorities, e => ((KPIWeightPriority)e).LevelName, currentEmployeeId.Value);
            await AddDeletedAsync(items, "QUESTIONRATINGSCALE", _uow.Perf.QuestionRatingScales, e => ((QuestionRatingScale)e).Name, currentEmployeeId.Value);
            await AddDeletedAsync(items, "LEVEL", _uow.HR.Levels, e => ((Level)e).Name, currentEmployeeId.Value);
            await AddDeletedAsync(items, "DEPARTMENT", _uow.HR.Departments, e => ((Department)e).Name, currentEmployeeId.Value);
            await AddDeletedAsync(items, "POSITION", _uow.HR.Positions, e => ((Position)e).Name, currentEmployeeId.Value);
            await AddDeletedAsync(items, "TEAM", _uow.HR.Teams, e => ((Team)e).Name, currentEmployeeId.Value);
            await AddDeletedAsync(items, "CATEGORY", _uow.Shared.Categories, e => ((Category)e).Name, currentEmployeeId.Value);

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
            IGenericRepository<T> repo, Func<T, string> getName, long currentEmployeeId) where T : class
        {
            var deleted = await repo.GetAllDeletedAsync();
            foreach (var e in deleted)
            {
                var sd = e as ISoftDeletable;
                if (sd?.DeletedBy != currentEmployeeId) continue;
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


    }
}
