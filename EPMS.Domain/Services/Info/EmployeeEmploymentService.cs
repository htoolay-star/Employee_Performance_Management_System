using AutoMapper;
using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.EmployeeInfo;
using EPMS.Domain.Interface.IService.Info;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.EmployeeInfoDTOs;
using EPMS.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace EPMS.Domain.Services.Info;

public class EmployeeEmploymentService : IEmployeeEmploymentService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public EmployeeEmploymentService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<SuccessResponse<IEnumerable<EmployeeEmploymentDto>>> GetAllAsync()
    {
        var employments = await _uow.Info.EmployeeEmployments.GetAllAsync();
        var dtos = _mapper.Map<IEnumerable<EmployeeEmploymentDto>>(employments);
        return SuccessResponse<IEnumerable<EmployeeEmploymentDto>>.Ok(dtos, "Employee employments retrieved successfully.");
    }

    public async Task<SuccessResponse<EmployeeEmploymentDto>> GetByIdAsync(long id)
    {
        var employment = await _uow.Info.EmployeeEmployments.GetByIdAsync(id);

        if (employment == null)
            return SuccessResponse<EmployeeEmploymentDto>.Fail($"Employee employment with ID '{id}' was not found.", ErrorType.NotFound);

        var dto = _mapper.Map<EmployeeEmploymentDto>(employment);
        return SuccessResponse<EmployeeEmploymentDto>.Ok(dto, "Employee employment retrieved successfully.");
    }

    public async Task<SuccessResponse<EmployeeEmploymentDto>> GetByEmployeeIdAsync(long employeeId)
    {
        var employment = await _uow.Info.EmployeeEmployments.GetByEmployeeIdAsync(employeeId);

        if (employment == null)
            return SuccessResponse<EmployeeEmploymentDto>.Fail($"Employment for employee ID '{employeeId}' was not found.", ErrorType.NotFound);

        var dto = _mapper.Map<EmployeeEmploymentDto>(employment);
        return SuccessResponse<EmployeeEmploymentDto>.Ok(dto, "Employee employment retrieved successfully.");
    }

    public async Task<SuccessResponse<long>> CreateAsync(CreateEmployeeEmploymentDto dto)
    {
        // Check if profile exists
        var profile = await _uow.Info.EmployeeProfiles.GetByIdAsync(dto.EmployeeId);
        if (profile == null)
            return SuccessResponse<long>.Fail($"Employee profile with ID '{dto.EmployeeId}' was not found.", ErrorType.NotFound);

        // Check if employment already exists for this employee
        var existing = await _uow.Info.EmployeeEmployments.GetByEmployeeIdAsync(dto.EmployeeId);
        if (existing != null)
            return SuccessResponse<long>.Fail($"Employment already exists for employee ID '{dto.EmployeeId}'. Use Update instead.", ErrorType.Conflict);

        // Validate foreign keys exist
        if (!await _uow.HR.Departments.ExistsByIdAsync(dto.DepartmentId))
            return SuccessResponse<long>.Fail($"Department with ID '{dto.DepartmentId}' was not found.", ErrorType.NotFound);

        if (!await _uow.HR.Positions.ExistsByIdAsync(dto.PositionId))
            return SuccessResponse<long>.Fail($"Position with ID '{dto.PositionId}' was not found.", ErrorType.NotFound);

        var employment = new EmployeeEmployment(
            dto.EmployeeId, 
            dto.DepartmentId, 
            dto.ParentDepartmentId, 
            dto.PositionId, 
            dto.EmploymentStatus);

        _uow.Info.EmployeeEmployments.Add(employment);
        await _uow.CompleteAsync();

        return SuccessResponse<long>.Ok(employment.Id, "Employee employment created successfully.");
    }

    public async Task<SuccessResponse> UpdateAsync(long id, UpdateEmployeeEmploymentDto dto)
    {
        var employment = await _uow.Info.EmployeeEmployments.GetByIdAsync(id);

        if (employment == null)
            return SuccessResponse.Fail($"Employee employment with ID '{id}' was not found.", ErrorType.NotFound);

        // Note: The entity doesn't have direct update methods, so we'd need to handle this differently
        // For now, we'll use the existing methods or add new ones to the entity
        
        if (dto.DateOfConfirmation.HasValue)
            employment.ConfirmEmployment(dto.DateOfConfirmation.Value);
        
        if (!string.IsNullOrEmpty(dto.ProductProject))
            employment.AssignProject(dto.ProductProject);
        
        if (dto.DateOfIncrement.HasValue)
            employment.LogIncrement(dto.DateOfIncrement.Value);

        await _uow.CompleteAsync();
        return SuccessResponse.Ok("Employee employment updated successfully.");
    }

    public async Task<SuccessResponse> DeleteAsync(long id)
    {
        var employment = await _uow.Info.EmployeeEmployments.GetByIdAsync(id);

        if (employment == null)
            return SuccessResponse.Fail($"Employee employment with ID '{id}' was not found.", ErrorType.NotFound);

        _uow.Info.EmployeeEmployments.Delete(employment);
        await _uow.CompleteAsync();

        return SuccessResponse.Ok("Employee employment deleted successfully.");
    }
}
