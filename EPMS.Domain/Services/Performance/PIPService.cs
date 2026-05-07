using AutoMapper;
using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Performance;
using EPMS.Domain.Interface.IService.Performance;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.PerformanceDTOs.PIPDTOs;
using EPMS.Shared.Enums;
using static EPMS.Shared.Constants.ServiceResponseMessages;
using static EPMS.Shared.Constants.PIPStatuses;

namespace EPMS.Domain.Services.Performance
{
    public class PIPService : IPIPService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public PIPService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<SuccessResponse<IEnumerable<PIPDto>>> GetAllAsync()
        {
            var pips = await _uow.Perf.PIPs.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<PIPDto>>(pips);
            return SuccessResponse<IEnumerable<PIPDto>>.Ok(dtos, PIPMsg.RetrievedAll);
        }

        public async Task<SuccessResponse<IEnumerable<PIPDto>>> GetActivePIPsAsync()
        {
            var pips = await _uow.Perf.PIPs.GetActivePIPsAsync();
            var dtos = _mapper.Map<IEnumerable<PIPDto>>(pips);
            return SuccessResponse<IEnumerable<PIPDto>>.Ok(dtos, PIPMsg.RetrievedActive);
        }

        public async Task<SuccessResponse<IEnumerable<PIPDto>>> GetByEmployeeIdAsync(long employeeId)
        {
            var pips = await _uow.Perf.PIPs.GetByEmployeeIdAsync(employeeId);
            var dtos = _mapper.Map<IEnumerable<PIPDto>>(pips);
            return SuccessResponse<IEnumerable<PIPDto>>.Ok(dtos, PIPMsg.RetrievedAll);
        }

        public async Task<SuccessResponse<IEnumerable<PIPDto>>> GetByManagerIdAsync(long managerId)
        {
            var pips = await _uow.Perf.PIPs.GetByManagerIdAsync(managerId);
            var dtos = _mapper.Map<IEnumerable<PIPDto>>(pips);
            return SuccessResponse<IEnumerable<PIPDto>>.Ok(dtos, PIPMsg.RetrievedAll);
        }

        public async Task<SuccessResponse<PIPDto>> GetByIdAsync(long id)
        {
            var pip = await _uow.Perf.PIPs.GetByIdAsync(id);

            if (pip == null)
                return SuccessResponse<PIPDto>.Fail(PIPMsg.NotFound(id), ErrorType.NotFound);

            var dto = _mapper.Map<PIPDto>(pip);
            return SuccessResponse<PIPDto>.Ok(dto, PIPMsg.Retrieved);
        }

        public async Task<SuccessResponse<long>> CreateAsync(CreatePIPDto dto)
        {
            var pip = new PIP(dto.EmployeeId, dto.ManagerId, dto.StartDate, dto.EndDate, dto.Reason, dto.AppraisalId);

            _uow.Perf.PIPs.Add(pip);
            await _uow.CompleteAsync();

            return SuccessResponse<long>.Ok(pip.Id, PIPMsg.Created);
        }

        public async Task<SuccessResponse> UpdateAsync(long id, UpdatePIPDto dto)
        {
            var pip = await _uow.Perf.PIPs.GetByIdAsync(id);

            if (pip == null)
                return SuccessResponse.Fail(PIPMsg.NotFound(id), ErrorType.NotFound);

            if (pip.Status == Successful || pip.Status == Failed)
                return SuccessResponse.Fail(PIPMsg.AlreadyConcluded, ErrorType.Validation);

            pip.ExtendPIP(dto.EndDate, dto.Reason);

            _uow.Perf.PIPs.Update(pip);
            await _uow.CompleteAsync();

            return SuccessResponse.Ok(PIPMsg.Updated);
        }

        public async Task<SuccessResponse> DeleteAsync(long id)
        {
            var pip = await _uow.Perf.PIPs.GetByIdAsync(id);

            if (pip == null)
                return SuccessResponse.Fail(PIPMsg.NotFound(id), ErrorType.NotFound);

            _uow.Perf.PIPs.Delete(pip);
            await _uow.CompleteAsync();

            return SuccessResponse.Ok(PIPMsg.Deleted);
        }

        public async Task<SuccessResponse> ConcludeAsync(long id, bool isSuccessful, string? notes)
        {
            var pip = await _uow.Perf.PIPs.GetByIdAsync(id);

            if (pip == null)
                return SuccessResponse.Fail(PIPMsg.NotFound(id), ErrorType.NotFound);

            if (pip.Status == Successful || pip.Status == Failed)
                return SuccessResponse.Fail(PIPMsg.AlreadyConcluded, ErrorType.Validation);

            pip.ConcludePIP(isSuccessful, notes);

            _uow.Perf.PIPs.Update(pip);
            await _uow.CompleteAsync();

            return SuccessResponse.Ok(PIPMsg.Concluded);
        }

        public async Task<SuccessResponse> ExtendAsync(long id, DateOnly newEndDate, string reason)
        {
            var pip = await _uow.Perf.PIPs.GetByIdAsync(id);

            if (pip == null)
                return SuccessResponse.Fail(PIPMsg.NotFound(id), ErrorType.NotFound);

            if (pip.Status == Successful || pip.Status == Failed)
                return SuccessResponse.Fail(PIPMsg.AlreadyConcluded, ErrorType.Validation);

            pip.ExtendPIP(newEndDate, reason);

            _uow.Perf.PIPs.Update(pip);
            await _uow.CompleteAsync();

            return SuccessResponse.Ok(PIPMsg.Extended);
        }
    }
}