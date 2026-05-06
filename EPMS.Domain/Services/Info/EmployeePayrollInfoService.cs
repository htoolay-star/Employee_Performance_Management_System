using AutoMapper;
using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.EmployeeInfo;
using EPMS.Domain.Interface.IService.Info;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.EmployeeInfoDTOs;
using EPMS.Shared.Enums;

namespace EPMS.Domain.Services.Info;

public class EmployeePayrollInfoService : IEmployeePayrollInfoService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public EmployeePayrollInfoService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<SuccessResponse<IEnumerable<EmployeePayrollInfoDto>>> GetAllAsync()
    {
        var payrolls = await _uow.Info.EmployeePayrollInfos.GetAllAsync();
        var dtos = _mapper.Map<IEnumerable<EmployeePayrollInfoDto>>(payrolls);
        return SuccessResponse<IEnumerable<EmployeePayrollInfoDto>>.Ok(dtos, "Employee payroll info retrieved successfully.");
    }

    public async Task<SuccessResponse<EmployeePayrollInfoDto>> GetByIdAsync(long id)
    {
        var payroll = await _uow.Info.EmployeePayrollInfos.GetByIdAsync(id);

        if (payroll == null)
            return SuccessResponse<EmployeePayrollInfoDto>.Fail($"Employee payroll info with ID '{id}' was not found.", ErrorType.NotFound);

        var dto = _mapper.Map<EmployeePayrollInfoDto>(payroll);
        return SuccessResponse<EmployeePayrollInfoDto>.Ok(dto, "Employee payroll info retrieved successfully.");
    }

    public async Task<SuccessResponse<EmployeePayrollInfoDto>> GetByEmployeeIdAsync(long employeeId)
    {
        var payroll = await _uow.Info.EmployeePayrollInfos.GetByEmployeeIdAsync(employeeId);

        if (payroll == null)
            return SuccessResponse<EmployeePayrollInfoDto>.Fail($"Payroll info for employee ID '{employeeId}' was not found.", ErrorType.NotFound);

        var dto = _mapper.Map<EmployeePayrollInfoDto>(payroll);
        return SuccessResponse<EmployeePayrollInfoDto>.Ok(dto, "Employee payroll info retrieved successfully.");
    }

    public async Task<SuccessResponse<long>> CreateAsync(CreateEmployeePayrollInfoDto dto)
    {
        // Check if profile exists
        var profile = await _uow.Info.EmployeeProfiles.GetByIdAsync(dto.EmployeeId);
        if (profile == null)
            return SuccessResponse<long>.Fail($"Employee profile with ID '{dto.EmployeeId}' was not found.", ErrorType.NotFound);

        // Check if payroll info already exists for this employee
        var existing = await _uow.Info.EmployeePayrollInfos.GetByEmployeeIdAsync(dto.EmployeeId);
        if (existing != null)
            return SuccessResponse<long>.Fail($"Payroll info already exists for employee ID '{dto.EmployeeId}'. Use Update instead.", ErrorType.Conflict);

        if (dto.Salary < 0)
            return SuccessResponse<long>.Fail("Salary cannot be negative.", ErrorType.Validation);

        var payroll = new EmployeePayrollInfo(dto.EmployeeId, dto.Salary, dto.Currency);

        _uow.Info.EmployeePayrollInfos.Add(payroll);
        await _uow.CompleteAsync();

        return SuccessResponse<long>.Ok(payroll.Id, "Employee payroll info created successfully.");
    }

    public async Task<SuccessResponse> UpdateAsync(long id, UpdateEmployeePayrollInfoDto dto)
    {
        var payroll = await _uow.Info.EmployeePayrollInfos.GetByIdAsync(id);

        if (payroll == null)
            return SuccessResponse.Fail($"Employee payroll info with ID '{id}' was not found.", ErrorType.NotFound);

        if (dto.Salary < 0)
            return SuccessResponse.Fail("Salary cannot be negative.", ErrorType.Validation);

        await _uow.CompleteAsync();
        return SuccessResponse.Ok("Employee payroll info updated successfully.");
    }

    public async Task<SuccessResponse> DeleteAsync(long id)
    {
        var payroll = await _uow.Info.EmployeePayrollInfos.GetByIdAsync(id);

        if (payroll == null)
            return SuccessResponse.Fail($"Employee payroll info with ID '{id}' was not found.", ErrorType.NotFound);

        _uow.Info.EmployeePayrollInfos.Delete(payroll);
        await _uow.CompleteAsync();

        return SuccessResponse.Ok("Employee payroll info deleted successfully.");
    }
}
