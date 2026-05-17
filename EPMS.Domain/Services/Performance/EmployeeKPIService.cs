using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Performance;
using EPMS.Shared.Constants;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.Performance.EmployeeKPI;
using EPMS.Shared.Enums;
using Mapster;
using static EPMS.Shared.Constants.ServiceResponseMessages;

namespace EPMS.Domain.Services.Performance
{
    public interface IEmployeeKPIService
    {
        Task<SuccessResponse<IEnumerable<EmployeeKPIDto>>> GetAllAsync();
        Task<SuccessResponse<EmployeeKPIDto>> GetByIdAsync(long id);
        Task<SuccessResponse<IEnumerable<EmployeeKPIDto>>> GetByEmployeeAndCycleAsync(long employeeId, long cycleId);
        Task<SuccessResponse<IEnumerable<EmployeeKPIDto>>> GetByCycleAsync(long cycleId);
        Task<SuccessResponse<long>> CreateAsync(CreateEmployeeKPIDto dto);
        Task<SuccessResponse> UpdateAsync(long id, UpdateEmployeeKPIDto dto);
        Task<SuccessResponse> RestoreAsync(long id);
        Task<SuccessResponse> DeleteAsync(long id);
    }

    public class EmployeeKPIService : IEmployeeKPIService
    {
        private readonly IUnitOfWork _uow;

        public EmployeeKPIService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<SuccessResponse<IEnumerable<EmployeeKPIDto>>> GetAllAsync()
        {
            var items = await _uow.Perf.EmployeeKPIs.GetAllAsync();
            var dtos = await ResolveNamesAsync(items);
            return SuccessResponse<IEnumerable<EmployeeKPIDto>>.Ok(dtos, EmployeeKPIMsg.RetrievedAll);
        }

        public async Task<SuccessResponse<EmployeeKPIDto>> GetByIdAsync(long id)
        {
            var item = await _uow.Perf.EmployeeKPIs.GetByIdAsync(id);
            if (item == null)
                return SuccessResponse<EmployeeKPIDto>.Fail(EmployeeKPIMsg.NotFound(id), ErrorType.NotFound);

            var dtos = await ResolveNamesAsync(new[] { item });
            return SuccessResponse<EmployeeKPIDto>.Ok(dtos.First(), EmployeeKPIMsg.Retrieved);
        }

        public async Task<SuccessResponse<IEnumerable<EmployeeKPIDto>>> GetByEmployeeAndCycleAsync(long employeeId, long cycleId)
        {
            var items = await _uow.Perf.EmployeeKPIs.GetByEmployeeAndCycleAsync(employeeId, cycleId);
            var dtos = await ResolveNamesAsync(items);
            return SuccessResponse<IEnumerable<EmployeeKPIDto>>.Ok(dtos, EmployeeKPIMsg.Retrieved);
        }

        public async Task<SuccessResponse<IEnumerable<EmployeeKPIDto>>> GetByCycleAsync(long cycleId)
        {
            var items = await _uow.Perf.EmployeeKPIs.GetByCycleAsync(cycleId);
            var dtos = await ResolveNamesAsync(items);
            return SuccessResponse<IEnumerable<EmployeeKPIDto>>.Ok(dtos, EmployeeKPIMsg.Retrieved);
        }

        public async Task<SuccessResponse<long>> CreateAsync(CreateEmployeeKPIDto dto)
        {
            if (await _uow.Perf.EmployeeKPIs.ExistsAsync(dto.EmployeeId, dto.KPIId, dto.CycleId))
                return SuccessResponse<long>.Fail(EmployeeKPIMsg.DuplicateEntry, ErrorType.Conflict);

            var priority = await _uow.Perf.KPIWeightPriorities.GetByIdAsync(dto.PriorityId);
            if (priority == null)
                return SuccessResponse<long>.Fail(EmployeeKPIMsg.PriorityNotFound, ErrorType.NotFound);

            var currentTotal = await _uow.Perf.EmployeeKPIs.GetTotalWeightageAsync(dto.EmployeeId, dto.CycleId);
            if (currentTotal + dto.Weightage > 100)
                return SuccessResponse<long>.Fail(EmployeeKPIMsg.WeightExceeded(currentTotal, dto.Weightage), ErrorType.Validation);

            var employeeKPI = new EmployeeKPI(priority, dto.EmployeeId, dto.KPIId, dto.CycleId, priority.Id, dto.Weightage, dto.TargetValue, dto.TargetUnit);

            _uow.Perf.EmployeeKPIs.Add(employeeKPI);
            await _uow.CompleteAsync();

            var newTotal = currentTotal + dto.Weightage;
            var message = newTotal == 100 ? EmployeeKPIMsg.Created : EmployeeKPIMsg.WeightNotComplete(newTotal);
            return SuccessResponse<long>.Ok(employeeKPI.Id, message);
        }

        public async Task<SuccessResponse> UpdateAsync(long id, UpdateEmployeeKPIDto dto)
        {
            var employeeKPI = await _uow.Perf.EmployeeKPIs.GetByIdAsync(id);
            if (employeeKPI == null)
                return SuccessResponse.Fail(EmployeeKPIMsg.NotFound(id), ErrorType.NotFound);

            var priority = await _uow.Perf.KPIWeightPriorities.GetByIdAsync(dto.PriorityId);
            if (priority == null)
                return SuccessResponse.Fail(EmployeeKPIMsg.PriorityNotFound, ErrorType.NotFound);

            var currentTotal = await _uow.Perf.EmployeeKPIs.GetTotalWeightageAsync(employeeKPI.EmployeeId, employeeKPI.CycleId, id);
            if (currentTotal + dto.Weightage > 100)
                return SuccessResponse.Fail(EmployeeKPIMsg.WeightExceeded(currentTotal, dto.Weightage), ErrorType.Validation);

            employeeKPI.Update(priority, dto.Weightage, dto.TargetValue, dto.TargetUnit);

            _uow.Perf.EmployeeKPIs.Update(employeeKPI);
            await _uow.CompleteAsync();

            var newTotal = currentTotal + dto.Weightage;
            var message = newTotal == 100 ? EmployeeKPIMsg.Updated : EmployeeKPIMsg.WeightNotComplete(newTotal);
            return SuccessResponse.Ok(message);
        }

        public async Task<SuccessResponse> DeleteAsync(long id)
        {
            var employeeKPI = await _uow.Perf.EmployeeKPIs.GetByIdAsync(id);
            if (employeeKPI == null)
                return SuccessResponse.Fail(EmployeeKPIMsg.NotFound(id), ErrorType.NotFound);

            _uow.Perf.EmployeeKPIs.Delete(employeeKPI);
            await _uow.CompleteAsync();

            return SuccessResponse.Ok(EmployeeKPIMsg.Deleted);
        }

        private async Task<List<EmployeeKPIDto>> ResolveNamesAsync(IEnumerable<EmployeeKPI> items)
        {
            var result = new List<EmployeeKPIDto>();
            foreach (var item in items)
            {
                var dto = item.Adapt<EmployeeKPIDto>();
                var emp = await _uow.Info.EmployeeProfiles.GetByIdAsync(item.EmployeeId);
                dto.EmployeeName = emp?.StaffName ?? item.EmployeeId.ToString();
                result.Add(dto);
            }
            return result;
        }

        public async Task<SuccessResponse> RestoreAsync(long id)
        {
            var entity = await _uow.Perf.EmployeeKPIs.GetByIdAsync(id);
            if (entity == null)
                return SuccessResponse.Fail(EmployeeKPIMsg.NotFound(id), ErrorType.NotFound);
            if (!entity.IsDeleted)
                return SuccessResponse.Fail("Item is not deleted.", ErrorType.Validation);
            entity.IsDeleted = false;
            entity.DeletedAt = null;
            entity.DeletedBy = null;
            _uow.Perf.EmployeeKPIs.Update(entity);
            await _uow.CompleteAsync();
            return SuccessResponse.Ok(EmployeeKPIMsg.Updated);
        }
    }
}