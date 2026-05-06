using AutoMapper;
using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.EmployeeInfo;
using EPMS.Domain.Interface.IService.Info;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.EmployeeInfoDTOs;
using EPMS.Shared.Enums;

namespace EPMS.Domain.Services.Info;

public class EmployeeFamilyInfoService : IEmployeeFamilyInfoService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public EmployeeFamilyInfoService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<SuccessResponse<IEnumerable<EmployeeFamilyInfoDto>>> GetAllAsync()
    {
        var infos = await _uow.Info.EmployeeFamilyInfos.GetAllAsync();
        var dtos = _mapper.Map<IEnumerable<EmployeeFamilyInfoDto>>(infos);
        return SuccessResponse<IEnumerable<EmployeeFamilyInfoDto>>.Ok(dtos, "Employee family info retrieved successfully.");
    }

    public async Task<SuccessResponse<EmployeeFamilyInfoDto>> GetByIdAsync(long id)
    {
        var info = await _uow.Info.EmployeeFamilyInfos.GetByIdAsync(id);

        if (info == null)
            return SuccessResponse<EmployeeFamilyInfoDto>.Fail($"Employee family info with ID '{id}' was not found.", ErrorType.NotFound);

        var dto = _mapper.Map<EmployeeFamilyInfoDto>(info);
        return SuccessResponse<EmployeeFamilyInfoDto>.Ok(dto, "Employee family info retrieved successfully.");
    }

    public async Task<SuccessResponse<EmployeeFamilyInfoDto>> GetByEmployeeIdAsync(long employeeId)
    {
        var info = await _uow.Info.EmployeeFamilyInfos.GetByEmployeeIdAsync(employeeId);

        if (info == null)
            return SuccessResponse<EmployeeFamilyInfoDto>.Fail($"Family info for employee ID '{employeeId}' was not found.", ErrorType.NotFound);

        var dto = _mapper.Map<EmployeeFamilyInfoDto>(info);
        return SuccessResponse<EmployeeFamilyInfoDto>.Ok(dto, "Employee family info retrieved successfully.");
    }

    public async Task<SuccessResponse<long>> CreateAsync(CreateEmployeeFamilyInfoDto dto)
    {
        // Check if profile exists
        var profile = await _uow.Info.EmployeeProfiles.GetByIdAsync(dto.EmployeeId);
        if (profile == null)
            return SuccessResponse<long>.Fail($"Employee profile with ID '{dto.EmployeeId}' was not found.", ErrorType.NotFound);

        // Check if family info already exists for this employee
        var existing = await _uow.Info.EmployeeFamilyInfos.GetByEmployeeIdAsync(dto.EmployeeId);
        if (existing != null)
            return SuccessResponse<long>.Fail($"Family info already exists for employee ID '{dto.EmployeeId}'. Use Update instead.", ErrorType.Conflict);

        var info = new EmployeeFamilyInfo(dto.EmployeeId);

        _uow.Info.EmployeeFamilyInfos.Add(info);
        await _uow.CompleteAsync();

        return SuccessResponse<long>.Ok(info.Id, "Employee family info created successfully.");
    }

    public async Task<SuccessResponse> UpdateAsync(long id, UpdateEmployeeFamilyInfoDto dto)
    {
        var info = await _uow.Info.EmployeeFamilyInfos.GetByIdAsync(id);

        if (info == null)
            return SuccessResponse.Fail($"Employee family info with ID '{id}' was not found.", ErrorType.NotFound);

        await _uow.CompleteAsync();
        return SuccessResponse.Ok("Employee family info updated successfully.");
    }

    public async Task<SuccessResponse> DeleteAsync(long id)
    {
        var info = await _uow.Info.EmployeeFamilyInfos.GetByIdAsync(id);

        if (info == null)
            return SuccessResponse.Fail($"Employee family info with ID '{id}' was not found.", ErrorType.NotFound);

        _uow.Info.EmployeeFamilyInfos.Delete(info);
        await _uow.CompleteAsync();

        return SuccessResponse.Ok("Employee family info deleted successfully.");
    }
}
