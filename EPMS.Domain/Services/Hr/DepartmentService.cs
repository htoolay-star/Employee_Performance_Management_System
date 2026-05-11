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

    public async Task<SuccessResponse<IEnumerable<LookUpDto>>> GetLookupAsync()
    {
        var cachedAllDepts = await _cacheService.GetAsync<IEnumerable<DepartmentDto>>(CacheKeys.Hr.AllDepartments());

        if (cachedAllDepts != null)
        {
            var lookupFromCache = cachedAllDepts.Select(x => new LookUpDto
            {
                Id = x.Id,
                Code = x.Code,
                IsActive = x.IsActive
            });
            return SuccessResponse<IEnumerable<LookUpDto>>.Ok(lookupFromCache, DeptMsg.RetrievedAll);
        }

        var tuples = await _uow.HR.Departments.GetLookupAsync();

        var dtos = tuples.Select(t => new LookUpDto
        {
            Id = t.Id,
            Code = t.Code,
            IsActive = t.IsActive
        }).ToList();

        return SuccessResponse<IEnumerable<LookUpDto>>.Ok(dtos, DeptMsg.RetrievedAll);
    }

    public async Task<SuccessResponse<IEnumerable<DepartmentDto>>> GetDepartmentWithTeamsAsync(long teamId)
    {
        var departments = await _uow.HR.Departments.GetDepartmentWithTeamsAsync(teamId);
        var dtos = departments.Adapt<IEnumerable<DepartmentDto>>();
        return SuccessResponse<IEnumerable<DepartmentDto>>.Ok(dtos, DeptMsg.RetrievedAll);
    }

    public async Task<SuccessResponse<IEnumerable<DepartmentDto>>> GetAllAsync()
    {
        var dtos = await _cacheService.GetOrCreateAsync(CacheKeys.Hr.AllDepartments(), async () =>
        {
            var departments = await _uow.HR.Departments.GetAllAsync();
            return departments.Adapt<IEnumerable<DepartmentDto>>();
        });
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

        var entity = new Department(dto.Code, dto.Name, dto.Description, dto.DeptHeadId);
        _uow.HR.Departments.Add(entity);
        await _uow.CompleteAsync();
        await _cacheService.RemoveAsync(CacheKeys.Hr.AllDepartments());
        return SuccessResponse<long>.Ok(entity.Id, DeptMsg.Created);
    }

    public async Task<SuccessResponse> UpdateAsync(long id, UpdateDepartmentDto dto)
    {
        var department = await _uow.HR.Departments.GetByIdAsync(id);

        if (department == null)
            return SuccessResponse.Fail(DeptMsg.NotFound(id), ErrorType.NotFound);

        department.SetDescription(dto.Description);
        department.SetDeptHead(dto.DeptHeadId);
        
        if (dto.IsActive) department.Reactivate();
        else department.Deactivate();

        await _uow.CompleteAsync();
        await _cacheService.RemoveAsync(CacheKeys.Hr.AllDepartments());
        return SuccessResponse.Ok(DeptMsg.Updated);
    }

    public async Task<SuccessResponse> DeleteAsync(long id)
    {
        var department = await _uow.HR.Departments.GetByIdAsync(id);

        if (department == null)
            return SuccessResponse.Fail(DeptMsg.NotFound(id), ErrorType.NotFound);

        if (await _uow.HR.Teams.AnyAsync(t => t.DepartmentId == id))
            return SuccessResponse.Fail(DeptMsg.InUse(id), ErrorType.Conflict);

        _uow.HR.Departments.Delete(department);
        await _uow.CompleteAsync();
        await _cacheService.RemoveAsync(CacheKeys.Hr.AllDepartments());
        return SuccessResponse.Ok(DeptMsg.Deleted);
    }
}
