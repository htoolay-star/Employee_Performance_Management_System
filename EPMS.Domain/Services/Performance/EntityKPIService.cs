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
        Task PropagatePositionKPIsToEmployeeAsync(long employeeId, long positionId);
        Task PropagatePositionKPIsForAllEmployeesAsync();
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

        private enum PropagationAction { Create, Update, Delete }

        private async Task PropagateToPositionEmployeesAsync(
            long entityKpiId, long kpiId, long priorityId, decimal weightage,
            decimal? targetValue, string? targetUnit, long positionId,
            PropagationAction action)
        {
            var activeCycle = await _uow.Perf.AppraisalCycles.GetCurrentCycleAsync();
            if (activeCycle == null)
                return;

            var employments = await _uow.Info.EmployeeEmployments.GetByPositionIdAsync(positionId);

            if (action == PropagationAction.Create)
            {
                var priority = await _uow.Perf.KPIWeightPriorities.GetByIdAsync(priorityId);
                foreach (var emp in employments)
                {
                    var exists = await _uow.Perf.EmployeeKPIs.ExistsAsync(
                        emp.EmployeeId, kpiId, activeCycle.Id);
                    if (!exists)
                    {
                        var employeeKpi = new EmployeeKPI(
                            priority, emp.EmployeeId, kpiId, activeCycle.Id,
                            priorityId, weightage, targetValue, targetUnit, entityKpiId);
                        _uow.Perf.EmployeeKPIs.Add(employeeKpi);
                    }
                }
            }
            else if (action == PropagationAction.Update)
            {
                var priority = await _uow.Perf.KPIWeightPriorities.GetByIdAsync(priorityId);
                foreach (var emp in employments)
                {
                    var existing = await _uow.Perf.EmployeeKPIs.FindAsync(
                        k => k.EntityKPIId == entityKpiId
                          && k.EmployeeId == emp.EmployeeId
                          && k.CycleId == activeCycle.Id
                          && !k.IsDeleted,
                        trackChanges: true);
                    if (existing != null)
                        existing.Update(priority, weightage, targetValue, targetUnit);
                }
            }
            else if (action == PropagationAction.Delete)
            {
                foreach (var emp in employments)
                {
                    var existing = await _uow.Perf.EmployeeKPIs.FindAllAsync(
                        k => k.EntityKPIId == entityKpiId
                          && k.EmployeeId == emp.EmployeeId
                          && !k.IsDeleted,
                        trackChanges: true);
                    foreach (var kpi in existing)
                        _uow.Perf.EmployeeKPIs.Delete(kpi);
                }
            }
        }

        public async Task PropagatePositionKPIsToEmployeeAsync(long employeeId, long positionId)
        {
            var activeCycle = await _uow.Perf.AppraisalCycles.GetCurrentCycleAsync();
            if (activeCycle == null)
                return;

            var entityKpis = await _uow.Perf.EntityKPIs.GetByEntityAsync(
                AppraisalConstants.EntityTypes.Position, positionId);

            var runningWeight = await _uow.Perf.EmployeeKPIs.GetTotalWeightageAsync(
                employeeId, activeCycle.Id);

            foreach (var entityKpi in entityKpis)
            {
                var exists = await _uow.Perf.EmployeeKPIs.ExistsAsync(
                    employeeId, entityKpi.KPIId, activeCycle.Id);
                if (!exists)
                {
                    if (runningWeight + entityKpi.Weightage > 100)
                        continue;

                    var priority = await _uow.Perf.KPIWeightPriorities.GetByIdAsync(entityKpi.PriorityId);
                    if (priority != null)
                    {
                        var employeeKpi = new EmployeeKPI(
                            priority, employeeId, entityKpi.KPIId, activeCycle.Id,
                            entityKpi.PriorityId, entityKpi.Weightage,
                            entityKpi.TargetValue, entityKpi.TargetUnit, entityKpi.Id);
                        _uow.Perf.EmployeeKPIs.Add(employeeKpi);
                        runningWeight += entityKpi.Weightage;
                    }
                }
            }
        }

        public async Task PropagatePositionKPIsForAllEmployeesAsync()
        {
            var activeCycle = await _uow.Perf.AppraisalCycles.GetCurrentCycleAsync();
            if (activeCycle == null)
                return;

            var employments = await _uow.Info.EmployeeEmployments.GetAllAsync();
            foreach (var emp in employments.Where(e => !e.IsDeleted))
                await PropagatePositionKPIsToEmployeeAsync(emp.EmployeeId, emp.PositionId);
        }

        public async Task<SuccessResponse<IEnumerable<EntityKPIDto>>> GetAllAsync()
        {
            var items = await _uow.Perf.EntityKPIs.GetAllWithIncludesAsync();
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

            var currentTotal = await _uow.Perf.EntityKPIs.GetTotalWeightageAsync(dto.EntityType, dto.EntityId);
            if (currentTotal + dto.Weightage > 100)
                return SuccessResponse<long>.Fail(EntityKPIMsg.WeightExceeded(currentTotal, dto.Weightage), ErrorType.Validation);

            var entity = new EntityKPI(dto.EntityType, dto.EntityId, dto.KPIId, priority, dto.Weightage, dto.TargetValue, dto.TargetUnit);

            _uow.Perf.EntityKPIs.Add(entity);

            if (dto.EntityType == AppraisalConstants.EntityTypes.Position)
            {
                await PropagateToPositionEmployeesAsync(
                    entity.Id, entity.KPIId, entity.PriorityId, entity.Weightage,
                    entity.TargetValue, entity.TargetUnit, dto.EntityId,
                    PropagationAction.Create);
            }

            await _uow.CompleteAsync();

            var newTotal = currentTotal + dto.Weightage;
            var message = newTotal == 100 ? EntityKPIMsg.Created : EntityKPIMsg.WeightNotComplete(newTotal);
            return SuccessResponse<long>.Ok(entity.Id, message);
        }

        public async Task<SuccessResponse> UpdateAsync(long id, UpdateEntityKPIDto dto)
        {
            var entity = await _uow.Perf.EntityKPIs.GetByIdAsync(id);
            if (entity == null)
                return SuccessResponse.Fail(EntityKPIMsg.NotFound(id), ErrorType.NotFound);

            var priority = await _uow.Perf.KPIWeightPriorities.GetByIdAsync(dto.PriorityId);
            if (priority == null)
                return SuccessResponse.Fail(EntityKPIMsg.PriorityNotFound, ErrorType.NotFound);

            var currentTotal = await _uow.Perf.EntityKPIs.GetTotalWeightageAsync(entity.EntityType, entity.EntityId, id);
            if (currentTotal + dto.Weightage > 100)
                return SuccessResponse.Fail(EntityKPIMsg.WeightExceeded(currentTotal, dto.Weightage), ErrorType.Validation);

            var entityType = entity.EntityType;
            var entityKpiId = entity.Id;
            var kpiId = entity.KPIId;
            var entityId = entity.EntityId;

            entity.Update(priority, dto.Weightage, dto.TargetValue, dto.TargetUnit);

            _uow.Perf.EntityKPIs.Update(entity);

            if (entityType == AppraisalConstants.EntityTypes.Position)
            {
                await PropagateToPositionEmployeesAsync(
                    entityKpiId, kpiId, dto.PriorityId, dto.Weightage,
                    dto.TargetValue, dto.TargetUnit, entityId,
                    PropagationAction.Update);
            }

            await _uow.CompleteAsync();

            var newTotal = currentTotal + dto.Weightage;
            var message = newTotal == 100 ? EntityKPIMsg.Updated : EntityKPIMsg.WeightNotComplete(newTotal);
            return SuccessResponse.Ok(message);
        }

        public async Task<SuccessResponse> DeleteAsync(long id)
        {
            var entity = await _uow.Perf.EntityKPIs.GetByIdAsync(id);
            if (entity == null)
                return SuccessResponse.Fail(EntityKPIMsg.NotFound(id), ErrorType.NotFound);

            var entityType = entity.EntityType;
            var entityKpiId = entity.Id;
            var kpiId = entity.KPIId;
            var entityId = entity.EntityId;

            _uow.Perf.EntityKPIs.Delete(entity);

            if (entityType == AppraisalConstants.EntityTypes.Position)
            {
                await PropagateToPositionEmployeesAsync(
                    entityKpiId, kpiId, 0, 0, null, null, entityId,
                    PropagationAction.Delete);
            }

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
            var entity = await _uow.Perf.EntityKPIs.GetByIdDeletedAsync(id);
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