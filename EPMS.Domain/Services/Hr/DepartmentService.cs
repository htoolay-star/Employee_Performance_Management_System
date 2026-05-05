using AutoMapper;
using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Hr;
using EPMS.Domain.Interface.Irepo.Hr;
using EPMS.Domain.Interfaces;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.DepartmentDTOs;
using EPMS.Shared.DTOs.TeamDTOs;
using EPMS.Shared.Enums;

namespace EPMS.Domain.Services.Hr;

public class DepartmentService : IDepartmentService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _uow;

    public DepartmentService(IMapper mapper, IUnitOfWork uow)
    {
        _mapper = mapper;
        _uow = uow;
    }

    public async Task<SuccessResponse<IEnumerable<DepartmentDto>>> GetAllAsync()
    {
        var departments = await _uow.HR.Departments.GetAllAsync();
        var dtos = _mapper.Map<IEnumerable<DepartmentDto>>(departments);
        return SuccessResponse<IEnumerable<DepartmentDto>>.Ok(dtos, "Departments retrieved successfully.");
    }

    public async Task<SuccessResponse<DepartmentDto>> GetByIdAsync(long id)
    {
        var department = await _uow.HR.Departments.GetDepartmentWithTeamsAsync(id);

        if (department is null)
            return SuccessResponse<DepartmentDto>.Fail($"Department with ID '{id}' was not found.", ErrorType.NotFound);

        var dto = _mapper.Map<DepartmentDto>(department);
        return SuccessResponse<DepartmentDto>.Ok(dto, "Department retrieved successfully.");
    }

    public async Task<SuccessResponse<long>> CreateAsync(CreateDepartmentDto dto)
    {
        if (await _uow.HR.Departments.ExistsByCodeAsync(dto.Code))
            return SuccessResponse<long>.Fail($"Department with code '{dto.Code}' already exists.", ErrorType.Conflict);

        if (await _uow.HR.Departments.ExistsByNameAsync(dto.Name))
            return SuccessResponse<long>.Fail($"Department with name '{dto.Name}' already exists.", ErrorType.Conflict);

        var entity = new Department(dto.Code, dto.Name);
        _uow.HR.Departments.Add(entity);
        await _uow.CompleteAsync();
        return SuccessResponse<long>.Ok(entity.Id, "Department created successfully.");
    }

    public async Task<SuccessResponse> UpdateAsync(long id, UpdateDepartmentDto dto)
    {
        var department = await _uow.HR.Departments.GetByIdAsync(id);

        if (department == null)
            return SuccessResponse.Fail($"Department with ID '{id}' was not found.", ErrorType.NotFound);

        if (department.Name != dto.Name && await _uow.HR.Departments.ExistsByNameAsync(dto.Name))
            return SuccessResponse.Fail($"Another department with name '{dto.Name}' already exists.", ErrorType.Conflict);

        department.Rename(dto.Name);
        
        if (dto.IsActive) department.Reactivate();
        else department.Deactivate();

        await _uow.CompleteAsync();
        return SuccessResponse.Ok("Department updated successfully.");
    }

    public async Task<SuccessResponse> DeleteAsync(long id)
    {
        var department = await _uow.HR.Departments.GetByIdAsync(id);

        if (department == null)
            return SuccessResponse.Fail($"Department with ID '{id}' was not found.", ErrorType.NotFound);

        _uow.HR.Departments.Delete(department);
        await _uow.CompleteAsync();
        return SuccessResponse.Ok("Department deleted successfully.");
    }

    public async Task<SuccessResponse<IEnumerable<TeamDto>>> GetTeamsForDepartmentAsync(long departmentId)
    {
        var department = await _uow.HR.Departments.GetByIdAsync(departmentId);

        if (department is null)
            return SuccessResponse<IEnumerable<TeamDto>>.Fail($"Department with ID '{departmentId}' was not found.", ErrorType.NotFound);

        var teams = await _uow.HR.Teams.GetTeamsByDepartmentAsync(departmentId);
        var dtos = _mapper.Map<IEnumerable<TeamDto>>(teams);
        return SuccessResponse<IEnumerable<TeamDto>>.Ok(dtos, "Teams retrieved successfully.");
    }

    public async Task<SuccessResponse> AddTeamToDepartmentAsync(long departmentId, string teamName)
    {
        var department = await _uow.HR.Departments.GetDepartmentWithTeamsAsync(departmentId);

        if (department is null)
            return SuccessResponse.Fail($"Department with ID '{departmentId}' was not found.", ErrorType.NotFound);

        if (await _uow.HR.Teams.ExistsByNameInDepartmentAsync(teamName, departmentId))
            return SuccessResponse.Fail($"Team with name '{teamName}' already exists in this department.", ErrorType.Conflict);

        department.AddTeam(teamName);
        await _uow.CompleteAsync();
        return SuccessResponse.Ok("Team added successfully.");
    }

    public async Task<SuccessResponse> RemoveTeamFromDepartmentAsync(long departmentId, long teamId)
    {
        var department = await _uow.HR.Departments.GetByIdAsync(departmentId);

        if (department is null)
            return SuccessResponse.Fail($"Department with ID '{departmentId}' was not found.", ErrorType.NotFound);

        var team = await _uow.HR.Teams.GetByIdAsync(teamId);

        if (team is null)
            return SuccessResponse.Fail($"Team with ID '{teamId}' was not found.", ErrorType.NotFound);

        if (team.DepartmentId != departmentId)
            return SuccessResponse.Fail($"Team '{teamId}' does not belong to department '{departmentId}'.", ErrorType.Conflict);

        _uow.HR.Teams.Delete(team);
        await _uow.CompleteAsync();
        return SuccessResponse.Ok("Team removed successfully.");
    }
}
