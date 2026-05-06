using AutoMapper;
using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Hr;
using EPMS.Domain.Interface.Irepo.Hr;
using EPMS.Domain.Interfaces;
using EPMS.Shared.Constants;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.DepartmentDTOs;
using EPMS.Shared.DTOs.TeamDTOs;
using EPMS.Shared.Enums;
using DeptMsg = EPMS.Shared.Constants.ServiceResponseMessages.DepartmentMsg;
using TeamMsg = EPMS.Shared.Constants.ServiceResponseMessages.TeamMsg;

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
        return SuccessResponse<IEnumerable<DepartmentDto>>.Ok(dtos, DeptMsg.RetrievedAll);
    }

    public async Task<SuccessResponse<DepartmentDto>> GetByIdAsync(long id)
    {
        var department = await _uow.HR.Departments.GetDepartmentWithTeamsAsync(id);

        if (department is null)
            return SuccessResponse<DepartmentDto>.Fail(DeptMsg.NotFound(id), ErrorType.NotFound);

        var dto = _mapper.Map<DepartmentDto>(department);
        return SuccessResponse<DepartmentDto>.Ok(dto, DeptMsg.Retrieved);
    }

    public async Task<SuccessResponse<long>> CreateAsync(CreateDepartmentDto dto)
    {
        if (await _uow.HR.Departments.ExistsByCodeAsync(dto.Code))
            return SuccessResponse<long>.Fail(string.Format(DeptMsg.DuplicateCode, dto.Code), ErrorType.Conflict);

        if (await _uow.HR.Departments.ExistsByNameAsync(dto.Name))
            return SuccessResponse<long>.Fail(string.Format(DeptMsg.DuplicateName, dto.Name), ErrorType.Conflict);

        var entity = new Department(dto.Code, dto.Name);
        _uow.HR.Departments.Add(entity);
        await _uow.CompleteAsync();
        return SuccessResponse<long>.Ok(entity.Id, DeptMsg.Created);
    }

    public async Task<SuccessResponse> UpdateAsync(long id, UpdateDepartmentDto dto)
    {
        var department = await _uow.HR.Departments.GetByIdAsync(id);

        if (department == null)
            return SuccessResponse.Fail(DeptMsg.NotFound(id), ErrorType.NotFound);

        if (department.Name != dto.Name && await _uow.HR.Departments.ExistsByNameAsync(dto.Name))
            return SuccessResponse.Fail(string.Format(DeptMsg.DuplicateNameOther, dto.Name), ErrorType.Conflict);

        department.Rename(dto.Name);
        
        if (dto.IsActive) department.Reactivate();
        else department.Deactivate();

        await _uow.CompleteAsync();
        return SuccessResponse.Ok(DeptMsg.Updated);
    }

    public async Task<SuccessResponse> DeleteAsync(long id)
    {
        var department = await _uow.HR.Departments.GetByIdAsync(id);

        if (department == null)
            return SuccessResponse.Fail(DeptMsg.NotFound(id), ErrorType.NotFound);

        _uow.HR.Departments.Delete(department);
        await _uow.CompleteAsync();
        return SuccessResponse.Ok(DeptMsg.Deleted);
    }

    public async Task<SuccessResponse<IEnumerable<TeamDto>>> GetTeamsForDepartmentAsync(long departmentId)
    {
        var department = await _uow.HR.Departments.GetByIdAsync(departmentId);

        if (department is null)
            return SuccessResponse<IEnumerable<TeamDto>>.Fail(TeamMsg.NotFoundForDepartment(departmentId), ErrorType.NotFound);

        var teams = await _uow.HR.Teams.GetTeamsByDepartmentAsync(departmentId);
        var dtos = _mapper.Map<IEnumerable<TeamDto>>(teams);
        return SuccessResponse<IEnumerable<TeamDto>>.Ok(dtos, TeamMsg.RetrievedAll);
    }

    public async Task<SuccessResponse> AddTeamToDepartmentAsync(long departmentId, string teamName)
    {
        var department = await _uow.HR.Departments.GetDepartmentWithTeamsAsync(departmentId);

        if (department is null)
            return SuccessResponse.Fail(TeamMsg.NotFoundForDepartment(departmentId), ErrorType.NotFound);

        if (await _uow.HR.Teams.ExistsByNameInDepartmentAsync(teamName, departmentId))
            return SuccessResponse.Fail(string.Format(TeamMsg.DuplicateName, teamName), ErrorType.Conflict);

        department.AddTeam(teamName);
        await _uow.CompleteAsync();
        return SuccessResponse.Ok(TeamMsg.Added);
    }

    public async Task<SuccessResponse> RemoveTeamFromDepartmentAsync(long departmentId, long teamId)
    {
        var department = await _uow.HR.Departments.GetByIdAsync(departmentId);

        if (department is null)
            return SuccessResponse.Fail(TeamMsg.NotFoundForDepartment(departmentId), ErrorType.NotFound);

        var team = await _uow.HR.Teams.GetByIdAsync(teamId);

        if (team is null)
            return SuccessResponse.Fail(TeamMsg.NotFound(teamId), ErrorType.NotFound);

        if (team.DepartmentId != departmentId)
            return SuccessResponse.Fail(TeamMsg.NotFoundInDepartment(teamId, departmentId), ErrorType.Conflict);

        _uow.HR.Teams.Delete(team);
        await _uow.CompleteAsync();
        return SuccessResponse.Ok(TeamMsg.Removed);
    }
}
