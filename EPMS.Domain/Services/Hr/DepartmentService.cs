using AutoMapper;
using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Hr;
using EPMS.Domain.Interface.Irepo.Hr;
using EPMS.Domain.Interface.IService.App;
using EPMS.Domain.Interfaces;
using EPMS.Shared.Constants;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.DepartmentDTOs;
using EPMS.Shared.DTOs.TeamDTOs;
using EPMS.Shared.Enums;
using Mapster;
using DeptMsg = EPMS.Shared.Constants.ServiceResponseMessages.DepartmentMsg;
using TeamMsg = EPMS.Shared.Constants.ServiceResponseMessages.TeamMsg;

namespace EPMS.Domain.Services.Hr;

public class DepartmentService : IDepartmentService
{
    private readonly IUnitOfWork _uow;
    private readonly ICacheService _cacheService;

    public DepartmentService(IUnitOfWork uow, ICacheService cacheService)
    {
        _uow = uow;
        _cacheService = cacheService;
    }

    public async Task<SuccessResponse<IEnumerable<DepartmentDto>>> GetDepartmentWithTeamsAsync(long teamId)
    {
        var departments = await _uow.HR.Departments.GetDepartmentWithTeamsAsync(teamId);
        var dtos = departments.Adapt<IEnumerable<DepartmentDto>>();
        return SuccessResponse<IEnumerable<DepartmentDto>>.Ok(dtos, DeptMsg.RetrievedAll);
    }

    public async Task<SuccessResponse<IEnumerable<DepartmentDto>>> GetAllAsync()
    {
        var departments = await _uow.HR.Departments.GetAllAsync();
        var dtos = departments.Adapt<IEnumerable<DepartmentDto>>();
        return SuccessResponse<IEnumerable<DepartmentDto>>.Ok(dtos, DeptMsg.RetrievedAll);
    }

    public async Task<SuccessResponse<DepartmentDto>> GetByIdAsync(long id)
    {
        var department = await _uow.HR.Departments.GetDepartmentWithTeamsAsync(id);

        if (department is null)
            return SuccessResponse<DepartmentDto>.Fail(DeptMsg.NotFound(id), ErrorType.NotFound);

        var dto = department.Adapt<DepartmentDto>();
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
}
