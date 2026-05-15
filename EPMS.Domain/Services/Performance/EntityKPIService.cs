using Mapster;
using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Hr;
using EPMS.Domain.Entities.Performance;
using EPMS.Shared.Constants;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.Performance.EntityKPI;
using EPMS.Shared.Enums;
using static EPMS.Shared.Constants.ServiceResponseMessages;

namespace EPMS.Domain.Services.Performance
{
    public interface IEntityKPIService
    {
        Task<SuccessResponse<IEnumerable<EntityKPIDto>>> GetAllAsync();
        Task<SuccessResponse<EntityKPIDto>> GetByIdAsync(long id);
        Task<SuccessResponse<IEnumerable<EntityKPIDto>>> GetByEntityAsync(string entityType, long entityId);
        Task<SuccessResponse<IEnumerable<EntityKPIDto>>> GetByEntityTypeAsync(string entityType);
        Task<SuccessResponse<long>> CreateAsync(CreateEntityKPIDto dto);
        Task<SuccessResponse> UpdateAsync(long id, UpdateEntityKPIDto dto);
        Task<SuccessResponse> RestoreAsync(long id);
        Task<SuccessResponse> DeleteAsync(long id);
    }

    public class EntityKPIService : IEntityKPIService
    {
        private readonly IUnitOfWork _uow;

        private static readonly HashSet<string> ValidEntityTypes = new(StringComparer.OrdinalIgnoreCase)
        {
            AppraisalConstants.EntityTypes.Position,
            AppraisalConstants.EntityTypes.Department,
            AppraisalConstants.EntityTypes.Team
        };

        public EntityKPIService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<SuccessResponse<IEnumerable<EntityKPIDto>>> GetAllAsync()
        {
            var items = await _uow.Perf.EntityKPIs.GetAllAsync();
            var dtos = await ResolveEntityNamesAsync(items);
            return SuccessResponse<IEnumerable<EntityKPIDto>>.Ok(dtos, EntityKPIMsg.RetrievedAll);
        }

        public async Task<SuccessResponse<EntityKPIDto>> GetByIdAsync(long id)
        {
            var item = await _uow.Perf.EntityKPIs.GetByIdAsync(id);
            if (item == null)
                return SuccessResponse<EntityKPIDto>.Fail(EntityKPIMsg.NotFound(id), ErrorType.NotFound);

            var dtos = await ResolveEntityNamesAsync(new[] { item });
            return SuccessResponse<EntityKPIDto>.Ok(dtos.First(), EntityKPIMsg.Retrieved);
        }

        public async Task<SuccessResponse<IEnumerable<EntityKPIDto>>> GetByEntityAsync(string entityType, long entityId)
        {
            var items = await _uow.Perf.EntityKPIs.GetByEntityAsync(entityType, entityId);
            var dtos = await ResolveEntityNamesAsync(items);
            return SuccessResponse<IEnumerable<EntityKPIDto>>.Ok(dtos, EntityKPIMsg.Retrieved);
        }

        public async Task<SuccessResponse<IEnumerable<EntityKPIDto>>> GetByEntityTypeAsync(string entityType)
        {
            var items = await _uow.Perf.EntityKPIs.GetByEntityTypeAsync(entityType);
            var dtos = await ResolveEntityNamesAsync(items);
            return SuccessResponse<IEnumerable<EntityKPIDto>>.Ok(dtos, EntityKPIMsg.Retrieved);
        }

        public async Task<SuccessResponse<long>> CreateAsync(CreateEntityKPIDto dto)
        {
            if (!ValidEntityTypes.Contains(dto.EntityType))
                return SuccessResponse<long>.Fail(EntityKPIMsg.InvalidEntityType, ErrorType.Validation);

            if (await _uow.Perf.EntityKPIs.ExistsAsync(dto.EntityType, dto.EntityId, dto.KPIId))
                return SuccessResponse<long>.Fail(EntityKPIMsg.DuplicateEntry, ErrorType.Conflict);

            var priority = await _uow.Perf.KPIWeightPriorities.GetByIdAsync(dto.PriorityId);
            if (priority == null)
                return SuccessResponse<long>.Fail(EntityKPIMsg.PriorityNotFound, ErrorType.NotFound);

            var entity = new EntityKPI(dto.EntityType, dto.EntityId, dto.KPIId, priority, dto.Weightage, dto.TargetValue, dto.TargetUnit);

            _uow.Perf.EntityKPIs.Add(entity);
            await _uow.CompleteAsync();

            return SuccessResponse<long>.Ok(entity.Id, EntityKPIMsg.Created);
        }

        public async Task<SuccessResponse> UpdateAsync(long id, UpdateEntityKPIDto dto)
        {
            var entity = await _uow.Perf.EntityKPIs.GetByIdAsync(id);
            if (entity == null)
                return SuccessResponse.Fail(EntityKPIMsg.NotFound(id), ErrorType.NotFound);

            var priority = await _uow.Perf.KPIWeightPriorities.GetByIdAsync(dto.PriorityId);
            if (priority == null)
                return SuccessResponse.Fail(EntityKPIMsg.PriorityNotFound, ErrorType.NotFound);

            entity.Update(priority, dto.Weightage, dto.TargetValue, dto.TargetUnit);

            _uow.Perf.EntityKPIs.Update(entity);
            await _uow.CompleteAsync();

            return SuccessResponse.Ok(EntityKPIMsg.Updated);
        }

        public async Task<SuccessResponse> DeleteAsync(long id)
        {
            var entity = await _uow.Perf.EntityKPIs.GetByIdAsync(id);
            if (entity == null)
                return SuccessResponse.Fail(EntityKPIMsg.NotFound(id), ErrorType.NotFound);

            _uow.Perf.EntityKPIs.Delete(entity);
            await _uow.CompleteAsync();

            return SuccessResponse.Ok(EntityKPIMsg.Deleted);
        }

        private async Task<List<EntityKPIDto>> ResolveEntityNamesAsync(IEnumerable<EntityKPI> items)
        {
            var result = new List<EntityKPIDto>();
            foreach (var item in items)
            {
                var dto = item.Adapt<EntityKPIDto>();
                dto.EntityName = await ResolveEntityNameAsync(item.EntityType, item.EntityId);
                result.Add(dto);
            }
            return result;
        }

        private async Task<string> ResolveEntityNameAsync(string entityType, long entityId)
        {
            var upper = entityType.ToUpperInvariant();
            if (upper == AppraisalConstants.EntityTypes.Position)
            {
                var pos = await _uow.HR.Positions.GetByIdAsync(entityId);
                return pos?.Name ?? entityId.ToString();
            }
            if (upper == AppraisalConstants.EntityTypes.Department)
            {
                var dept = await _uow.HR.Departments.GetByIdAsync(entityId);
                return dept?.Name ?? entityId.ToString();
            }
            if (upper == AppraisalConstants.EntityTypes.Team)
            {
                var team = await _uow.HR.Teams.GetByIdAsync(entityId);
                return team?.Name ?? entityId.ToString();
            }
            return entityId.ToString();
        }

        public async Task<SuccessResponse> RestoreAsync(long id)
        {
            var entity = await _uow.Perf.EntityKPIs.GetByIdAsync(id);
            if (entity == null)
                return SuccessResponse.Fail(EntityKPIMsg.NotFound(id), ErrorType.NotFound);
            if (!entity.IsDeleted)
                return SuccessResponse.Fail("Item is not deleted.", ErrorType.Validation);
            entity.IsDeleted = false;
            entity.DeletedAt = null;
            entity.DeletedBy = null;
            _uow.Perf.EntityKPIs.Update(entity);
            await _uow.CompleteAsync();
            return SuccessResponse.Ok(EntityKPIMsg.Updated);
        }
    }
}