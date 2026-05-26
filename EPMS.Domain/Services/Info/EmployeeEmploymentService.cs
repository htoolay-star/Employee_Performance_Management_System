using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.EmployeeInfo;
using EPMS.Domain.Interface.IService.Info;
using EPMS.Domain.Services.Performance;
using EPMS.Shared.Constants;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.EmployeeInfoDTOs;
using EPMS.Shared.Enums;
using Mapster;
using static EPMS.Shared.Constants.ServiceResponseMessages;

namespace EPMS.Domain.Services.Info;

public class EmployeeEmploymentService : IEmployeeEmploymentService
{
    private readonly IUnitOfWork _uow;
    private readonly TimeProvider _timeProvider;
    private readonly IEntityKPIService _kpiService;

    public EmployeeEmploymentService(IUnitOfWork uow, TimeProvider timeProvider, IEntityKPIService kpiService)
    {
        _uow = uow;
        _timeProvider = timeProvider;
        _kpiService = kpiService;
    }

    public async Task<SuccessResponse<IEnumerable<EmployeeEmploymentDto>>> GetAllAsync()
    {
        var employments = await _uow.Info.EmployeeEmployments.GetAllAsync();
        var dtos = employments.Adapt<IEnumerable<EmployeeEmploymentDto>>();
        return SuccessResponse<IEnumerable<EmployeeEmploymentDto>>.Ok(dtos, EmployeeEmploymentMsg.RetrievedAll);
    }

    public async Task<SuccessResponse<EmployeeEmploymentDto>> GetByIdAsync(long id)
    {
        var employment = await _uow.Info.EmployeeEmployments.GetByIdAsync(id);

        if (employment == null)
            return SuccessResponse<EmployeeEmploymentDto>.Fail(EmployeeEmploymentMsg.NotFound(id), ErrorType.NotFound);

        var dto = employment.Adapt<EmployeeEmploymentDto>();
        return SuccessResponse<EmployeeEmploymentDto>.Ok(dto, EmployeeEmploymentMsg.Retrieved);
    }

    public async Task<SuccessResponse<EmployeeEmploymentDto>> GetByEmployeeIdAsync(Guid employeePublicId)
    {
        var employee = await _uow.Info.EmployeeProfiles.GetByPublicIdAsync(employeePublicId);
        if (employee == null)
            return SuccessResponse<EmployeeEmploymentDto>.Fail(EmployeeProfileMsg.NotFound(employeePublicId), ErrorType.NotFound);

        var employment = await _uow.Info.EmployeeEmployments.GetByEmployeeIdAsync(employee.Id);

        if (employment == null)
            return SuccessResponse<EmployeeEmploymentDto>.Fail(EmployeeEmploymentMsg.NotFound(employeePublicId), ErrorType.NotFound);

        var dto = employment.Adapt<EmployeeEmploymentDto>();
        return SuccessResponse<EmployeeEmploymentDto>.Ok(dto, EmployeeEmploymentMsg.Retrieved);
    }

    public async Task<SuccessResponse<long>> CreateAsync(CreateEmployeeEmploymentDto dto)
    {
        // Check if profile exists
        var profile = await _uow.Info.EmployeeProfiles.GetByIdAsync(dto.EmployeeId);
        if (profile == null)
            return SuccessResponse<long>.Fail(EmployeeProfileMsg.NotFound(dto.EmployeeId), ErrorType.NotFound);

        // Check if employment already exists for this employee
        var existing = await _uow.Info.EmployeeEmployments.GetByEmployeeIdAsync(dto.EmployeeId);
        if (existing != null)
            return SuccessResponse<long>.Fail(EmployeeEmploymentMsg.Retrieved, ErrorType.Conflict);

        // Validate foreign keys exist
        if (!await _uow.HR.Departments.ExistsByIdAsync(dto.DepartmentId))
            return SuccessResponse<long>.Fail(DepartmentMsg.NotFound(dto.DepartmentId), ErrorType.NotFound);

        if (!await _uow.HR.Positions.ExistsByIdAsync(dto.PositionId))
            return SuccessResponse<long>.Fail(PositionMsg.NotFound(dto.PositionId), ErrorType.NotFound);

        var employment = new EmployeeEmployment(
            dto.EmployeeId,
            dto.DepartmentId,
            dto.ParentDepartmentId,
            dto.PositionId,
            dto.EmploymentStatus);
        employment.UpdateDetails(
            dto.DepartmentId, dto.ParentDepartmentId, dto.PositionId,
            dto.TeamId, dto.DirectManagerId, dto.EmploymentStatus,
            dto.StaffType, dto.ProbationMonth, dto.Shift, dto.FingerPrintId, dto.MobileAttendance);

        _uow.Info.EmployeeEmployments.Add(employment);

        await _kpiService.PropagatePositionKPIsToEmployeeAsync(dto.EmployeeId, dto.PositionId);
        await _uow.CompleteAsync();

        return SuccessResponse<long>.Ok(employment.Id, EmployeeEmploymentMsg.Created);
    }

    public async Task<SuccessResponse> UpdateAsync(long id, UpdateEmployeeEmploymentDto dto)
    {
        var employment = await _uow.Info.EmployeeEmployments.GetByIdAsync(id);

        if (employment == null)
            return SuccessResponse.Fail(EmployeeEmploymentMsg.NotFound(id), ErrorType.NotFound);

        // Capture old values before update for history
        var oldDepartmentId = employment.DepartmentId;
        var oldPositionId = employment.PositionId;
        var oldManagerId = employment.DirectManagerId;
        var oldStatus = employment.EmploymentStatus;

        // Update main employment details
        employment.UpdateDetails(
            dto.DepartmentId, dto.ParentDepartmentId, dto.PositionId,
            dto.TeamId, dto.DirectManagerId, dto.EmploymentStatus,
            dto.StaffType, dto.ProbationMonth,
            dto.Shift, dto.FingerPrintId, dto.MobileAttendance);

        // Handle date-related updates
        if (dto.DateOfConfirmation.HasValue)
            employment.ConfirmEmployment(dto.DateOfConfirmation.Value);

        if (!string.IsNullOrEmpty(dto.ProductProject))
            employment.AssignProject(dto.ProductProject);

        if (dto.DateOfIncrement.HasValue)
            employment.LogIncrement(dto.DateOfIncrement.Value);

        // Auto-create employment history record when key fields change
        bool keyFieldsChanged = oldDepartmentId != employment.DepartmentId
            || oldPositionId != employment.PositionId
            || oldManagerId != employment.DirectManagerId
            || !string.Equals(oldStatus, employment.EmploymentStatus, StringComparison.Ordinal);

        if (keyFieldsChanged)
        {
            var history = new EmployeeEmploymentHistory(
                employment.EmployeeId,
                employment.DepartmentId,
                employment.PositionId,
                employment.DirectManagerId,
                employment.EmploymentStatus,
                DateOnly.FromDateTime(_timeProvider.GetUtcNow().DateTime),
                _timeProvider,
                "Employment details updated");

            _uow.Info.EmployeeEmploymentHistories.Add(history);

            if (oldPositionId != employment.PositionId)
            {
                var oldEntityKpis = await _uow.Perf.EntityKPIs.GetByEntityAsync(
                    AppraisalConstants.EntityTypes.Position, oldPositionId);
                foreach (var ekpi in oldEntityKpis)
                {
                    var linked = await _uow.Perf.EmployeeKPIs.FindAsync(
                        k => k.EntityKPIId == ekpi.Id
                          && k.EmployeeId == employment.EmployeeId
                          && !k.IsDeleted,
                        trackChanges: true);
                    if (linked != null)
                        _uow.Perf.EmployeeKPIs.Delete(linked);
                }

                await _kpiService.PropagatePositionKPIsToEmployeeAsync(employment.EmployeeId, employment.PositionId);
            }
        }

        await _uow.CompleteAsync();
        return SuccessResponse.Ok(EmployeeEmploymentMsg.Updated);
    }

    public async Task<SuccessResponse> DeleteAsync(long id)
    {
        var employment = await _uow.Info.EmployeeEmployments.GetByIdAsync(id);

        if (employment == null)
            return SuccessResponse.Fail(EmployeeEmploymentMsg.NotFound(id), ErrorType.NotFound);

        _uow.Info.EmployeeEmployments.Delete(employment);
        await _uow.CompleteAsync();

        return SuccessResponse.Ok(EmployeeEmploymentMsg.Deleted);
    }
}
