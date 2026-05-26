using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Performance;
using EPMS.Domain.Interface.IService.Performance;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.PerformanceDTOs.PIPObjectiveDTOs;
using EPMS.Shared.Enums;
using Mapster;
using static EPMS.Shared.Constants.ServiceResponseMessages;
namespace EPMS.Domain.Services.Performance;

public class PIPObjectiveService : IPIPObjectiveService
{
    private readonly IUnitOfWork _uow;

    public PIPObjectiveService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<SuccessResponse<IEnumerable<PIPObjectiveDto>>> GetAllAsync()
    {
        var objectives = await _uow.Perf.PIPObjectives.GetAllAsync();
        var dtos = objectives.Adapt<IEnumerable<PIPObjectiveDto>>();
        return SuccessResponse<IEnumerable<PIPObjectiveDto>>.Ok(dtos, PIPObjectiveMsg.RetrievedAll);
    }

    public async Task<SuccessResponse<PIPObjectiveDto>> GetByIdAsync(long id)
    {
        var objective = await _uow.Perf.PIPObjectives.GetByIdAsync(id);

        if (objective == null)
            return SuccessResponse<PIPObjectiveDto>.Fail(PIPObjectiveMsg.NotFound(id), ErrorType.NotFound);

        var dto = objective.Adapt<PIPObjectiveDto>();
        return SuccessResponse<PIPObjectiveDto>.Ok(dto, PIPObjectiveMsg.Retrieved);
    }

    public async Task<SuccessResponse<IEnumerable<PIPObjectiveDto>>> GetByPIPIdAsync(long pipId)
    {
        var objectives = await _uow.Perf.PIPObjectives.GetByPIPIdAsync(pipId);
        var dtos = objectives.Adapt<IEnumerable<PIPObjectiveDto>>();
        return SuccessResponse<IEnumerable<PIPObjectiveDto>>.Ok(dtos, PIPObjectiveMsg.RetrievedByPIP);
    }

    public async Task<SuccessResponse<long>> CreateAsync(CreatePIPObjectiveDto dto)
    {
        var pip = await _uow.Perf.PIPs.GetByIdAsync(dto.PIPId);
        if (pip == null)
            return SuccessResponse<long>.Fail(PIPMsg.NotFound(dto.PIPId), ErrorType.NotFound);

        var objective = new PIPObjective(dto.PIPId, dto.Title, dto.SuccessCriteria, dto.Description);

        _uow.Perf.PIPObjectives.Add(objective);
        await _uow.CompleteAsync();

        return SuccessResponse<long>.Ok(objective.Id, PIPObjectiveMsg.Created);
    }

    public async Task<SuccessResponse> UpdateAsync(long id, UpdatePIPObjectiveDto dto)
    {
        var objective = await _uow.Perf.PIPObjectives.GetByIdAsync(id);

        if (objective == null)
            return SuccessResponse.Fail(PIPObjectiveMsg.NotFound(id), ErrorType.NotFound);

        if (dto.Title != null || dto.SuccessCriteria != null || dto.Description != null)
        {
            objective.UpdateDetails(
                dto.Title ?? objective.Title,
                dto.SuccessCriteria ?? objective.SuccessCriteria,
                dto.Description);
        }

        if (!string.IsNullOrWhiteSpace(dto.Status))
            objective.EvaluateObjective(dto.Status, dto.ManagerComment);

        await _uow.CompleteAsync();
        return SuccessResponse.Ok(PIPObjectiveMsg.Updated);
    }

    public async Task<SuccessResponse> DeleteAsync(long id)
    {
        var objective = await _uow.Perf.PIPObjectives.GetByIdAsync(id);

        if (objective == null)
            return SuccessResponse.Fail(PIPObjectiveMsg.NotFound(id), ErrorType.NotFound);

        _uow.Perf.PIPObjectives.Delete(objective);
        await _uow.CompleteAsync();

        return SuccessResponse.Ok(PIPObjectiveMsg.Deleted);
    }
}
