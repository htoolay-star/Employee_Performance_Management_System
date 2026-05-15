using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Performance;
using EPMS.Shared.Constants;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.Performance.DeptKPI;
using EPMS.Shared.Enums;
using static EPMS.Shared.Constants.ServiceResponseMessages;

using Mapster;
namespace EPMS.Domain.Services.Performance
{
    public interface IDeptKPIService
    {
        Task<SuccessResponse<IEnumerable<DeptKPIDto>>> GetAllAsync();
        Task<SuccessResponse<DeptKPIDto>> GetByIdAsync(long id);
        Task<SuccessResponse<IEnumerable<DeptKPIDto>>> GetByDeptIdAsync(long deptId);
        Task<SuccessResponse<long>> CreateAsync(CreateDeptKPIDto dto);
        Task<SuccessResponse> UpdateAsync(long id, UpdateDeptKPIDto dto);
        Task<SuccessResponse> DeleteAsync(long id);
    }

    public class DeptKPIService : IDeptKPIService
    {
        private readonly IUnitOfWork _uow;
        
        public DeptKPIService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<SuccessResponse<IEnumerable<DeptKPIDto>>> GetAllAsync()
        {
            var items = await _uow.Perf.DeptKPIs.GetAllAsync();
            var dtos = items.Adapt<IEnumerable<DeptKPIDto>>();
            return SuccessResponse<IEnumerable<DeptKPIDto>>.Ok(dtos, DeptKPIMsg.RetrievedAll);
        }

        public async Task<SuccessResponse<DeptKPIDto>> GetByIdAsync(long id)
        {
            var item = await _uow.Perf.DeptKPIs.GetByIdAsync(id);
            if (item == null)
                return SuccessResponse<DeptKPIDto>.Fail(DeptKPIMsg.NotFound(id), ErrorType.NotFound);

            var dto = item.Adapt<DeptKPIDto>();
            return SuccessResponse<DeptKPIDto>.Ok(dto, DeptKPIMsg.Retrieved);
        }

        public async Task<SuccessResponse<IEnumerable<DeptKPIDto>>> GetByDeptIdAsync(long deptId)
        {
            var items = await _uow.Perf.DeptKPIs.GetByDeptIdAsync(deptId);
            var dtos = items.Adapt<IEnumerable<DeptKPIDto>>();
            return SuccessResponse<IEnumerable<DeptKPIDto>>.Ok(dtos, DeptKPIMsg.RetrievedByDept);
        }

        public async Task<SuccessResponse<long>> CreateAsync(CreateDeptKPIDto dto)
        {
            if (await _uow.Perf.DeptKPIs.ExistsAsync(dto.DeptId, dto.KPIId))
                return SuccessResponse<long>.Fail(DeptKPIMsg.DuplicateEntry, ErrorType.Conflict);

            var priority = await _uow.Perf.KPIWeightPriorities.GetByIdAsync(dto.PriorityId);
            if (priority == null)
                return SuccessResponse<long>.Fail(DeptKPIMsg.PriorityNotFound, ErrorType.NotFound);

            var entity = new DeptKPI(dto.DeptId, dto.KPIId, priority, dto.Weightage, dto.TargetValue, dto.TargetUnit);

            _uow.Perf.DeptKPIs.Add(entity);
            await _uow.CompleteAsync();

            return SuccessResponse<long>.Ok(entity.Id, DeptKPIMsg.Created);
        }

        public async Task<SuccessResponse> UpdateAsync(long id, UpdateDeptKPIDto dto)
        {
            var entity = await _uow.Perf.DeptKPIs.GetByIdAsync(id);
            if (entity == null)
                return SuccessResponse.Fail(DeptKPIMsg.NotFound(id), ErrorType.NotFound);

            var priority = await _uow.Perf.KPIWeightPriorities.GetByIdAsync(dto.PriorityId);
            if (priority == null)
                return SuccessResponse.Fail(DeptKPIMsg.PriorityNotFound, ErrorType.NotFound);

            entity.Update(priority, dto.Weightage, dto.TargetValue, dto.TargetUnit);

            _uow.Perf.DeptKPIs.Update(entity);
            await _uow.CompleteAsync();

            return SuccessResponse.Ok(DeptKPIMsg.Updated);
        }

        public async Task<SuccessResponse> DeleteAsync(long id)
        {
            var entity = await _uow.Perf.DeptKPIs.GetByIdAsync(id);
            if (entity == null)
                return SuccessResponse.Fail(DeptKPIMsg.NotFound(id), ErrorType.NotFound);

            _uow.Perf.DeptKPIs.Delete(entity);
            await _uow.CompleteAsync();

            return SuccessResponse.Ok(DeptKPIMsg.Deleted);
        }
    }
}
