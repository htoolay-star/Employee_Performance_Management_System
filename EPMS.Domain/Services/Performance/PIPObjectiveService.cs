using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Performance;
using EPMS.Domain.Interface.IService.App;
using EPMS.Domain.Interface.IService.Performance;
using EPMS.Shared.Constants;
using EPMS.Shared.DTOs.AppDTOs;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.PerformanceDTOs.PIPObjectiveDTOs;
using EPMS.Shared.Enums;
using Mapster;
using static EPMS.Shared.Constants.ServiceResponseMessages;
namespace EPMS.Domain.Services.Performance;

public class PIPObjectiveService : IPIPObjectiveService
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentEmployeeContextService _currentEmployee;
    private readonly INotificationService _notificationService;

    public PIPObjectiveService(
        IUnitOfWork uow,
        ICurrentEmployeeContextService currentEmployee,
        INotificationService notificationService)
    {
        _uow = uow;
        _currentEmployee = currentEmployee;
        _notificationService = notificationService;
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

    public async Task<SuccessResponse> MarkCompleteAsync(long id)
    {
        var objective = await _uow.Perf.PIPObjectives.GetByIdAsync(id);
        if (objective == null)
            return SuccessResponse.Fail(PIPObjectiveMsg.NotFound(id), ErrorType.NotFound);

        var pip = await _uow.Perf.PIPs.GetByIdAsync(objective.PIPId);
        if (pip == null)
            return SuccessResponse.Fail(PIPMsg.NotFound(objective.PIPId), ErrorType.NotFound);

        var employeeId = await _currentEmployee.GetEmployeeIdAsync();
        if (pip.EmployeeId != employeeId)
            return SuccessResponse.Fail("Only the assigned employee can mark objectives as completed.", ErrorType.Forbidden);

        objective.EvaluateObjective(ObjectiveStatuses.Completed, null);
        await _uow.CompleteAsync();

        await NotifyManagerAsync(pip, objective);

        return SuccessResponse.Ok(PIPObjectiveMsg.Updated);
    }

    private async Task NotifyManagerAsync(PIP pip, PIPObjective objective)
    {
        try
        {
            var manager = await _uow.Info.EmployeeProfiles.GetByIdAsync(pip.ManagerId);
            var employee = await _uow.Info.EmployeeProfiles.GetByIdAsync(pip.EmployeeId);
            if (manager?.UserId == null) return;

            var employeeName = employee?.StaffName ?? $"Employee #{pip.EmployeeId}";
            var title = "PIP Objective Completed";
            var message = $"{employeeName} has marked objective \"{objective.Title}\" as completed.";
            var redirectUrl = "/performance/my-pips";

            await _notificationService.CreateAsync(new CreateNotificationDto
            {
                ToUserId = manager.UserId.Value,
                Title = title,
                Message = message,
                Type = "INFO",
                RedirectUrl = redirectUrl
            });
        }
        catch
        {
            // Notification failure should not block the objective completion
        }
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
