using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.EmployeeInfo;
using EPMS.Domain.Interface.IService.Info;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.EmployeeInfoDTOs;
using EPMS.Shared.Enums;
using Mapster;
using static EPMS.Shared.Constants.ServiceResponseMessages;

namespace EPMS.Domain.Services.Info;

public class EmployeePayrollInfoService : IEmployeePayrollInfoService
{
    private readonly IUnitOfWork _uow;
    private readonly TimeProvider _timeProvider;

    public EmployeePayrollInfoService(IUnitOfWork uow, TimeProvider timeProvider)
    {
        _uow = uow;
        _timeProvider = timeProvider;
    }

    public async Task<SuccessResponse<IEnumerable<EmployeePayrollInfoDto>>> GetAllAsync()
    {
        var payrolls = await _uow.Info.EmployeePayrollInfos.GetAllAsync();
        var dtos = payrolls.Adapt<IEnumerable<EmployeePayrollInfoDto>>();
        return SuccessResponse<IEnumerable<EmployeePayrollInfoDto>>.Ok(dtos, EmployeePayrollInfoMsg.RetrievedAll);
    }

    public async Task<SuccessResponse<EmployeePayrollInfoDto>> GetByIdAsync(long id)
    {
        var payroll = await _uow.Info.EmployeePayrollInfos.GetByIdAsync(id);

        if (payroll == null)
            return SuccessResponse<EmployeePayrollInfoDto>.Fail(EmployeePayrollInfoMsg.NotFound(id), ErrorType.NotFound);

        var dto = payroll.Adapt<EmployeePayrollInfoDto>();
        return SuccessResponse<EmployeePayrollInfoDto>.Ok(dto, EmployeePayrollInfoMsg.Retrieved);
    }

    public async Task<SuccessResponse<EmployeePayrollInfoDto>> GetByEmployeeIdAsync(long employeeId)
    {
        var payroll = await _uow.Info.EmployeePayrollInfos.GetByEmployeeIdAsync(employeeId);

        if (payroll == null)
            return SuccessResponse<EmployeePayrollInfoDto>.Fail(EmployeePayrollInfoMsg.NotFound(employeeId), ErrorType.NotFound);

        var dto = payroll.Adapt<EmployeePayrollInfoDto>();
        return SuccessResponse<EmployeePayrollInfoDto>.Ok(dto, EmployeePayrollInfoMsg.Retrieved);
    }

    public async Task<SuccessResponse<long>> CreateAsync(CreateEmployeePayrollInfoDto dto)
    {
        // Check if profile exists
        var profile = await _uow.Info.EmployeeProfiles.GetByIdAsync(dto.EmployeeId);
        if (profile == null)
            return SuccessResponse<long>.Fail(EmployeeProfileMsg.NotFound(dto.EmployeeId), ErrorType.NotFound);

        // Check if payroll info already exists for this employee
        var existing = await _uow.Info.EmployeePayrollInfos.GetByEmployeeIdAsync(dto.EmployeeId);
        if (existing != null)
            return SuccessResponse<long>.Fail(EmployeePayrollInfoMsg.Retrieved, ErrorType.Conflict);

        if (dto.Salary < 0)
            return SuccessResponse<long>.Fail(EmployeePayrollInfoMsg.SalaryNegative, ErrorType.Validation);

        var payroll = new EmployeePayrollInfo(dto.EmployeeId, dto.Salary, dto.Currency);

        _uow.Info.EmployeePayrollInfos.Add(payroll);
        await _uow.CompleteAsync();

        return SuccessResponse<long>.Ok(payroll.Id, EmployeePayrollInfoMsg.Created);
    }

    public async Task<SuccessResponse> UpdateAsync(long id, UpdateEmployeePayrollInfoDto dto)
    {
        var payroll = await _uow.Info.EmployeePayrollInfos.GetByIdAsync(id);

        if (payroll == null)
            return SuccessResponse.Fail(EmployeePayrollInfoMsg.NotFound(id), ErrorType.NotFound);

        if (dto.Salary < 0)
            return SuccessResponse.Fail(EmployeePayrollInfoMsg.SalaryNegative, ErrorType.Validation);

        var oldSalary = payroll.Salary;

        if (dto.Salary != payroll.Salary || dto.CostAllocate != null || dto.PayByBacklog != null)
            payroll.UpdatePayrollDetails(dto.Salary, dto.CostAllocate, dto.PayByBacklog);

        if (dto.TaxStatus != null || dto.TaxNo != null)
            payroll.UpdateTaxInfo(dto.TaxStatus, dto.TaxNo);

        if (dto.SSBStatus != null || dto.SSCBNo != null)
            payroll.UpdateSSBInfo(dto.SSBStatus, dto.SSCBNo);

        if (dto.ComplianceEarnedPoints != null || dto.ComplianceBalancePoints != null)
            payroll.UpdateCompliancePoints(dto.ComplianceEarnedPoints, dto.ComplianceBalancePoints);

        // Auto-create salary history when salary changes
        if (oldSalary != payroll.Salary)
        {
            var history = new EmployeeSalaryHistory(
                payroll.EmployeeId,
                oldSalary,
                payroll.Salary,
                DateOnly.FromDateTime(_timeProvider.GetUtcNow().DateTime),
                $"Salary updated from {oldSalary:N2} to {payroll.Salary:N2}",
                _timeProvider);

            _uow.Info.EmployeeSalaryHistories.Add(history);
        }

        await _uow.CompleteAsync();
        return SuccessResponse.Ok(EmployeePayrollInfoMsg.Updated);
    }

    public async Task<SuccessResponse> DeleteAsync(long id)
    {
        var payroll = await _uow.Info.EmployeePayrollInfos.GetByIdAsync(id);

        if (payroll == null)
            return SuccessResponse.Fail(EmployeePayrollInfoMsg.NotFound(id), ErrorType.NotFound);

        _uow.Info.EmployeePayrollInfos.Delete(payroll);
        await _uow.CompleteAsync();

        return SuccessResponse.Ok(EmployeePayrollInfoMsg.Deleted);
    }
}
