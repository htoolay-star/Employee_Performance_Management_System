using EPMS.Domain.Contracts;
using EPMS.Domain.Interface.IService.Info;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.EmployeeInfoDTOs;
using EPMS.Shared.Enums;
using Mapster;
using static EPMS.Shared.Constants.ServiceResponseMessages;

namespace EPMS.Domain.Services.Info;

public class EmployeeSalaryHistoryService : IEmployeeSalaryHistoryService
{
    private readonly IUnitOfWork _uow;
    private readonly TimeProvider _timeProvider;

    public EmployeeSalaryHistoryService(IUnitOfWork uow, TimeProvider timeProvider)
    {
        _uow = uow;
        _timeProvider = timeProvider;
    }

    public async Task<SuccessResponse<IEnumerable<EmployeeSalaryHistoryDto>>> GetAllAsync()
    {
        var histories = await _uow.Info.EmployeeSalaryHistories.GetAllAsync();
        var dtos = histories.Adapt<IEnumerable<EmployeeSalaryHistoryDto>>();
        return SuccessResponse<IEnumerable<EmployeeSalaryHistoryDto>>.Ok(dtos, EmployeeSalaryHistoryMsg.RetrievedAll);
    }

    public async Task<SuccessResponse<EmployeeSalaryHistoryDto>> GetByIdAsync(long id)
    {
        var history = await _uow.Info.EmployeeSalaryHistories.GetByIdAsync(id);

        if (history == null)
            return SuccessResponse<EmployeeSalaryHistoryDto>.Fail(EmployeeSalaryHistoryMsg.NotFound(id), ErrorType.NotFound);

        var dto = history.Adapt<EmployeeSalaryHistoryDto>();
        return SuccessResponse<EmployeeSalaryHistoryDto>.Ok(dto, EmployeeSalaryHistoryMsg.Retrieved);
    }

    public async Task<SuccessResponse<IEnumerable<EmployeeSalaryHistoryDto>>> GetByEmployeeIdAsync(long employeeId)
    {
        var histories = await _uow.Info.EmployeeSalaryHistories.GetByEmployeeIdAsync(employeeId);
        var dtos = histories.Adapt<IEnumerable<EmployeeSalaryHistoryDto>>();
        return SuccessResponse<IEnumerable<EmployeeSalaryHistoryDto>>.Ok(dtos, EmployeeSalaryHistoryMsg.RetrievedAll);
    }
}
