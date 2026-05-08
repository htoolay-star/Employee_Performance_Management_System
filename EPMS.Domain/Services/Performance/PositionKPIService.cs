using AutoMapper;
using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Performance;
using EPMS.Domain.Services.Performance;
using EPMS.Shared.Constants;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.Performance.PositionKPI;
using EPMS.Shared.Enums;
using System.Collections.Generic;
using static EPMS.Shared.Constants.ServiceResponseMessages;

namespace EPMS.Domain.Services.Performance
{
    public interface IPositionKPIService
    {
        Task<SuccessResponse<IEnumerable<PositionKPIDto>>> GetAllAsync();
        Task<SuccessResponse<PositionKPIDto>> GetByIdAsync(long id);
        Task<SuccessResponse<IEnumerable<PositionKPIDto>>> GetByPositionIdAsync(long positionId);
        Task<SuccessResponse<long>> CreateAsync(CreatePositionKPIDto dto);
        Task<SuccessResponse> UpdateAsync(long id, UpdatePositionKPIDto dto);
        Task<SuccessResponse> DeleteAsync(long id);
    }

    public class PositionKPIService : IPositionKPIService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public PositionKPIService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<SuccessResponse<IEnumerable<PositionKPIDto>>> GetAllAsync()
        {
            var positionKPIs = await _uow.Perf.PositionKPIs.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<PositionKPIDto>>(positionKPIs);
            return SuccessResponse<IEnumerable<PositionKPIDto>>.Ok(dtos, PositionKPIMsg.RetrievedAll);
        }

        public async Task<SuccessResponse<PositionKPIDto>> GetByIdAsync(long id)
        {
            var positionKPI = await _uow.Perf.PositionKPIs.GetByIdAsync(id);
            if (positionKPI == null)
                return SuccessResponse<PositionKPIDto>.Fail(PositionKPIMsg.NotFound(id), ErrorType.NotFound);

            var dto = _mapper.Map<PositionKPIDto>(positionKPI);
            return SuccessResponse<PositionKPIDto>.Ok(dto, PositionKPIMsg.Retrieved);
        }

        public async Task<SuccessResponse<IEnumerable<PositionKPIDto>>> GetByPositionIdAsync(long positionId)
        {
            var positionKPIs = await _uow.Perf.PositionKPIs.GetByPositionIdAsync(positionId);
            var dtos = _mapper.Map<IEnumerable<PositionKPIDto>>(positionKPIs);
            return SuccessResponse<IEnumerable<PositionKPIDto>>.Ok(dtos, PositionKPIMsg.RetrievedByPosition);
        }

        public async Task<SuccessResponse<long>> CreateAsync(CreatePositionKPIDto dto)
        {
            if (await _uow.Perf.PositionKPIs.ExistsAsync(dto.PositionId, dto.KPIId))
            {
                return SuccessResponse<long>.Fail(PositionKPIMsg.DuplicateEntry, ErrorType.Conflict);
            }

            var priority = await _uow.Perf.KPIWeightPriorities.GetByIdAsync(dto.PriorityId);
            if (priority == null)
                return SuccessResponse<long>.Fail(PositionKPIMsg.PriorityNotFound, ErrorType.NotFound);

            var positionKPI = new PositionKPI(dto.PositionId, dto.KPIId, priority, dto.Weightage, dto.TargetValue, dto.TargetUnit);

            _uow.Perf.PositionKPIs.Add(positionKPI);
            await _uow.CompleteAsync();

            return SuccessResponse<long>.Ok(positionKPI.Id, PositionKPIMsg.Created);
        }

        public async Task<SuccessResponse> UpdateAsync(long id, UpdatePositionKPIDto dto)
        {
            var positionKPI = await _uow.Perf.PositionKPIs.GetByIdAsync(id);
            if (positionKPI == null)
                return SuccessResponse.Fail(PositionKPIMsg.NotFound(id), ErrorType.NotFound);

            var priority = await _uow.Perf.KPIWeightPriorities.GetByIdAsync(dto.PriorityId);
            if (priority == null)
                return SuccessResponse.Fail(PositionKPIMsg.PriorityNotFound, ErrorType.NotFound);

            positionKPI.Update(priority, dto.Weightage, dto.TargetValue, dto.TargetUnit);

            _uow.Perf.PositionKPIs.Update(positionKPI);
            await _uow.CompleteAsync();

            return SuccessResponse.Ok(PositionKPIMsg.Updated);
        }

        public async Task<SuccessResponse> DeleteAsync(long id)
        {
            var positionKPI = await _uow.Perf.PositionKPIs.GetByIdAsync(id);
            if (positionKPI == null)
                return SuccessResponse.Fail(PositionKPIMsg.NotFound(id), ErrorType.NotFound);

            _uow.Perf.PositionKPIs.Delete(positionKPI);
            await _uow.CompleteAsync();

            return SuccessResponse.Ok(PositionKPIMsg.Deleted);
        }
    }
}