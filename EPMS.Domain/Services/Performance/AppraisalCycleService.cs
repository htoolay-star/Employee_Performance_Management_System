using AutoMapper;
using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Performance;
using EPMS.Domain.Interface.IService.Performance;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.PerformanceDTOs.AppraisalCycleDTOs;
using EPMS.Shared.Enums;
using static EPMS.Shared.Constants.ServiceResponseMessages;

namespace EPMS.Domain.Services.Performance;

public class AppraisalCycleService : IAppraisalCycleService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public AppraisalCycleService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<SuccessResponse<IEnumerable<AppraisalCycleDto>>> GetAllAsync()
    {
        var cycles = await _uow.Perf.AppraisalCycles.GetAllAsync();
        var dtos = _mapper.Map<IEnumerable<AppraisalCycleDto>>(cycles);
        return SuccessResponse<IEnumerable<AppraisalCycleDto>>.Ok(dtos, AppraisalCycleMsg.RetrievedAll);
    }

    public async Task<SuccessResponse<IEnumerable<AppraisalCycleDto>>> GetActiveCyclesAsync()
    {
        var cycles = await _uow.Perf.AppraisalCycles.GetActiveCyclesAsync();
        var dtos = _mapper.Map<IEnumerable<AppraisalCycleDto>>(cycles);
        return SuccessResponse<IEnumerable<AppraisalCycleDto>>.Ok(dtos, AppraisalCycleMsg.RetrievedActive);
    }

    public async Task<SuccessResponse<AppraisalCycleDto>> GetByIdAsync(long id)
    {
        var cycle = await _uow.Perf.AppraisalCycles.GetByIdAsync(id);

        if (cycle == null)
            return SuccessResponse<AppraisalCycleDto>.Fail(AppraisalCycleMsg.NotFound(id), ErrorType.NotFound);

        var dto = _mapper.Map<AppraisalCycleDto>(cycle);
        return SuccessResponse<AppraisalCycleDto>.Ok(dto, AppraisalCycleMsg.Retrieved);
    }

    public async Task<SuccessResponse<long>> CreateAsync(CreateAppraisalCycleDto dto)
    {
        var existing = await _uow.Perf.AppraisalCycles.GetByYearAndTypeAsync(dto.YearLabel, dto.AppraisalType);
        if (existing != null)
        {
            return SuccessResponse<long>.Fail(
                string.Format(AppraisalCycleMsg.DuplicateCycle, dto.YearLabel, dto.AppraisalType),
                ErrorType.Conflict);
        }

        var cycle = new AppraisalCycle(
            dto.Name,
            dto.AppraisalType,
            dto.CalendarType,
            dto.YearLabel,
            dto.EvaluationStartDate,
            dto.EvaluationEndDate,
            dto.WindowStartDate,
            dto.WindowEndDate
        );

        if (dto.SelfReviewStartDate.HasValue && dto.SelfReviewDeadline.HasValue)
        {
            cycle.ConfigureSelfReviewWindow(dto.SelfReviewStartDate.Value, dto.SelfReviewDeadline.Value);
        }

        if (dto.ManagerReviewStartDate.HasValue && dto.ManagerReviewDeadline.HasValue)
        {
            cycle.ConfigureManagerReviewWindow(dto.ManagerReviewStartDate.Value, dto.ManagerReviewDeadline.Value);
        }

        if (dto.PeerReviewStartDate.HasValue && dto.PeerReviewDeadline.HasValue)
        {
            cycle.ConfigurePeerReviewWindow(dto.PeerReviewStartDate.Value, dto.PeerReviewDeadline.Value);
        }

        _uow.Perf.AppraisalCycles.Add(cycle);
        await _uow.CompleteAsync();

        return SuccessResponse<long>.Ok(cycle.Id, AppraisalCycleMsg.Created);
    }

    public async Task<SuccessResponse> UpdateAsync(long id, UpdateAppraisalCycleDto dto)
    {
        var cycle = await _uow.Perf.AppraisalCycles.GetByIdAsync(id);
        if (cycle == null)
        {
            return SuccessResponse.Fail(AppraisalCycleMsg.NotFound(id), ErrorType.NotFound);
        }

        if (cycle.IsLocked)
        {
            return SuccessResponse.Fail(AppraisalCycleMsg.AlreadyLocked, ErrorType.Validation);
        }

        cycle.Update(dto.Name, dto.EvaluationStartDate, dto.EvaluationEndDate,
                     dto.WindowStartDate, dto.WindowEndDate);

        if (dto.SelfReviewStartDate.HasValue && dto.SelfReviewDeadline.HasValue)
        {
            cycle.ConfigureSelfReviewWindow(dto.SelfReviewStartDate.Value, dto.SelfReviewDeadline.Value);
        }

        if (dto.ManagerReviewStartDate.HasValue && dto.ManagerReviewDeadline.HasValue)
        {
            cycle.ConfigureManagerReviewWindow(dto.ManagerReviewStartDate.Value, dto.ManagerReviewDeadline.Value);
        }

        if (dto.PeerReviewStartDate.HasValue && dto.PeerReviewDeadline.HasValue)
        {
            cycle.ConfigurePeerReviewWindow(dto.PeerReviewStartDate.Value, dto.PeerReviewDeadline.Value);
        }

        _uow.Perf.AppraisalCycles.Update(cycle);
        await _uow.CompleteAsync();

        return SuccessResponse.Ok(AppraisalCycleMsg.Updated);
    }

    public async Task<SuccessResponse> DeleteAsync(long id)
    {
        var cycle = await _uow.Perf.AppraisalCycles.GetByIdAsync(id);
        if (cycle == null)
        {
            return SuccessResponse.Fail(AppraisalCycleMsg.NotFound(id), ErrorType.NotFound);
        }

        _uow.Perf.AppraisalCycles.Delete(cycle);
        await _uow.CompleteAsync();

        return SuccessResponse.Ok(AppraisalCycleMsg.Deleted);
    }

    public async Task<SuccessResponse> LockCycleAsync(long id)
    {
        var cycle = await _uow.Perf.AppraisalCycles.GetByIdAsync(id);
        if (cycle == null)
        {
            return SuccessResponse.Fail(AppraisalCycleMsg.NotFound(id), ErrorType.NotFound);
        }

        if (cycle.IsLocked)
        {
            return SuccessResponse.Fail(AppraisalCycleMsg.AlreadyLocked, ErrorType.Validation);
        }

        cycle.LockCycle();
        _uow.Perf.AppraisalCycles.Update(cycle);
        await _uow.CompleteAsync();

        return SuccessResponse.Ok(AppraisalCycleMsg.Locked);
    }

    public async Task<SuccessResponse> DeactivateAsync(long id)
    {
        var cycle = await _uow.Perf.AppraisalCycles.GetByIdAsync(id);
        if (cycle == null)
        {
            return SuccessResponse.Fail(AppraisalCycleMsg.NotFound(id), ErrorType.NotFound);
        }

        cycle.Deactivate();
        _uow.Perf.AppraisalCycles.Update(cycle);
        await _uow.CompleteAsync();

        return SuccessResponse.Ok(AppraisalCycleMsg.Deactivated);
    }
}
