using AutoMapper;
using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.EmployeeInfo;
using EPMS.Domain.Interface.IService.Info;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.EmployeeInfoDTOs;
using EPMS.Shared.Enums;

namespace EPMS.Domain.Services.Info;

public class EmployeeEmploymentHistoryService : IEmployeeEmploymentHistoryService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public EmployeeEmploymentHistoryService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<SuccessResponse<IEnumerable<EmployeeEmploymentHistoryDto>>> GetAllAsync()
    {
        var histories = await _uow.Info.EmployeeEmploymentHistories.GetAllAsync();
        var dtos = _mapper.Map<IEnumerable<EmployeeEmploymentHistoryDto>>(histories);
        return SuccessResponse<IEnumerable<EmployeeEmploymentHistoryDto>>.Ok(dtos, "Employment histories retrieved successfully.");
    }

    public async Task<SuccessResponse<EmployeeEmploymentHistoryDto>> GetByIdAsync(long id)
    {
        var history = await _uow.Info.EmployeeEmploymentHistories.GetByIdAsync(id);

        if (history == null)
            return SuccessResponse<EmployeeEmploymentHistoryDto>.Fail($"Employment history with ID '{id}' was not found.", ErrorType.NotFound);

        var dto = _mapper.Map<EmployeeEmploymentHistoryDto>(history);
        return SuccessResponse<EmployeeEmploymentHistoryDto>.Ok(dto, "Employment history retrieved successfully.");
    }

    public async Task<SuccessResponse<IEnumerable<EmployeeEmploymentHistoryDto>>> GetByEmployeeIdAsync(long employeeId)
    {
        var histories = await _uow.Info.EmployeeEmploymentHistories.GetByEmployeeIdAsync(employeeId);
        var dtos = _mapper.Map<IEnumerable<EmployeeEmploymentHistoryDto>>(histories);
        return SuccessResponse<IEnumerable<EmployeeEmploymentHistoryDto>>.Ok(dtos, "Employment histories retrieved successfully.");
    }

    public async Task<SuccessResponse<long>> CreateAsync(CreateEmployeeEmploymentHistoryDto dto)
    {
        // Check if profile exists
        var profile = await _uow.Info.EmployeeProfiles.GetByIdAsync(dto.EmployeeId);
        if (profile == null)
            return SuccessResponse<long>.Fail($"Employee profile with ID '{dto.EmployeeId}' was not found.", ErrorType.NotFound);

        // Validate department exists
        if (!await _uow.HR.Departments.ExistsByIdAsync(dto.DepartmentId))
            return SuccessResponse<long>.Fail($"Department with ID '{dto.DepartmentId}' was not found.", ErrorType.NotFound);

        // Validate position exists
        if (!await _uow.HR.Positions.ExistsByIdAsync(dto.PositionId))
            return SuccessResponse<long>.Fail($"Position with ID '{dto.PositionId}' was not found.", ErrorType.NotFound);

        var history = new EmployeeEmploymentHistory(
            dto.EmployeeId,
            dto.DepartmentId,
            dto.PositionId,
            dto.ManagerId,
            dto.EmploymentStatus,
            dto.EffectiveDate,
            dto.ChangeReason,
            dto.ChangedById);

        _uow.Info.EmployeeEmploymentHistories.Add(history);
        await _uow.CompleteAsync();

        return SuccessResponse<long>.Ok(history.Id, "Employment history created successfully.");
    }
}
