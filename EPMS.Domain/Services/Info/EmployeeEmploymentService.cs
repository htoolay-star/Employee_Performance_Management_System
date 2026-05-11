using AutoMapper;
using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.EmployeeInfo;
using EPMS.Domain.Interface.IService.Info;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.EmployeeInfoDTOs;
using EPMS.Shared.Enums;
using static EPMS.Shared.Constants.ServiceResponseMessages;

namespace EPMS.Domain.Services.Info;

public class EmployeeEmploymentService : IEmployeeEmploymentService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    private readonly TimeProvider _timeProvider;

    public EmployeeEmploymentService(IUnitOfWork uow, IMapper mapper, TimeProvider timeProvider)
    {
        _uow = uow;
        _mapper = mapper;
        _timeProvider = timeProvider;
    }

    public async Task<SuccessResponse<IEnumerable<EmployeeEmploymentDto>>> GetAllAsync()
    {
        var employments = await _uow.Info.EmployeeEmployments.GetAllAsync();
        var dtos = _mapper.Map<IEnumerable<EmployeeEmploymentDto>>(employments);
        return SuccessResponse<IEnumerable<EmployeeEmploymentDto>>.Ok(dtos, EmployeeEmploymentMsg.RetrievedAll);
    }

    public async Task<SuccessResponse<EmployeeEmploymentDto>> GetByIdAsync(long id)
    {
        var employment = await _uow.Info.EmployeeEmployments.GetByIdAsync(id);

        if (employment == null)
            return SuccessResponse<EmployeeEmploymentDto>.Fail(EmployeeEmploymentMsg.NotFound(id), ErrorType.NotFound);

        var dto = _mapper.Map<EmployeeEmploymentDto>(employment);
        return SuccessResponse<EmployeeEmploymentDto>.Ok(dto, EmployeeEmploymentMsg.Retrieved);
    }

    public async Task<SuccessResponse<EmployeeEmploymentDto>> GetByEmployeeIdAsync(long employeeId)
    {
        var employment = await _uow.Info.EmployeeEmployments.GetByEmployeeIdAsync(employeeId);

        if (employment == null)
            return SuccessResponse<EmployeeEmploymentDto>.Fail(EmployeeEmploymentMsg.NotFound(employeeId), ErrorType.NotFound);

        var dto = _mapper.Map<EmployeeEmploymentDto>(employment);
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

        _uow.Info.EmployeeEmployments.Add(employment);
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
