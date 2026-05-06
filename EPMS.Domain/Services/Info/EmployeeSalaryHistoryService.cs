using AutoMapper;
using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.EmployeeInfo;
using EPMS.Domain.Interface.IService.Info;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.EmployeeInfoDTOs;
using EPMS.Shared.Enums;

namespace EPMS.Domain.Services.Info;

public class EmployeeSalaryHistoryService : IEmployeeSalaryHistoryService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public EmployeeSalaryHistoryService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<SuccessResponse<IEnumerable<EmployeeSalaryHistoryDto>>> GetAllAsync()
    {
        var histories = await _uow.Info.EmployeeSalaryHistories.GetAllAsync();
        var dtos = _mapper.Map<IEnumerable<EmployeeSalaryHistoryDto>>(histories);
        return SuccessResponse<IEnumerable<EmployeeSalaryHistoryDto>>.Ok(dtos, "Salary histories retrieved successfully.");
    }

    public async Task<SuccessResponse<EmployeeSalaryHistoryDto>> GetByIdAsync(long id)
    {
        var history = await _uow.Info.EmployeeSalaryHistories.GetByIdAsync(id);

        if (history == null)
            return SuccessResponse<EmployeeSalaryHistoryDto>.Fail($"Salary history with ID '{id}' was not found.", ErrorType.NotFound);

        var dto = _mapper.Map<EmployeeSalaryHistoryDto>(history);
        return SuccessResponse<EmployeeSalaryHistoryDto>.Ok(dto, "Salary history retrieved successfully.");
    }

    public async Task<SuccessResponse<IEnumerable<EmployeeSalaryHistoryDto>>> GetByEmployeeIdAsync(long employeeId)
    {
        var histories = await _uow.Info.EmployeeSalaryHistories.GetByEmployeeIdAsync(employeeId);
        var dtos = _mapper.Map<IEnumerable<EmployeeSalaryHistoryDto>>(histories);
        return SuccessResponse<IEnumerable<EmployeeSalaryHistoryDto>>.Ok(dtos, "Salary histories retrieved successfully.");
    }

    public async Task<SuccessResponse<long>> CreateAsync(CreateEmployeeSalaryHistoryDto dto)
    {
        // Check if profile exists
        var profile = await _uow.Info.EmployeeProfiles.GetByIdAsync(dto.EmployeeId);
        if (profile == null)
            return SuccessResponse<long>.Fail($"Employee profile with ID '{dto.EmployeeId}' was not found.", ErrorType.NotFound);

        if (dto.PreviousAmount < 0 || dto.NewAmount < 0)
            return SuccessResponse<long>.Fail("Salary amounts cannot be negative.", ErrorType.Validation);

        if (string.IsNullOrWhiteSpace(dto.ChangeReason))
            return SuccessResponse<long>.Fail("Change reason is required.", ErrorType.Validation);

        var history = new EmployeeSalaryHistory(
            dto.EmployeeId,
            dto.PreviousAmount,
            dto.NewAmount,
            dto.EffectiveDate,
            dto.ChangeReason,
            dto.ApprovedById);

        _uow.Info.EmployeeSalaryHistories.Add(history);
        await _uow.CompleteAsync();

        return SuccessResponse<long>.Ok(history.Id, "Salary history created successfully.");
    }
}
