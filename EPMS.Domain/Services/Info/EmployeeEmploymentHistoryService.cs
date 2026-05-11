using AutoMapper;
using EPMS.Domain.Contracts;
using EPMS.Domain.Interface.IService.Info;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.EmployeeInfoDTOs;
using EPMS.Shared.Enums;
using static EPMS.Shared.Constants.ServiceResponseMessages;

namespace EPMS.Domain.Services.Info;

public class EmployeeEmploymentHistoryService : IEmployeeEmploymentHistoryService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    private readonly TimeProvider _timeProvider;

    public EmployeeEmploymentHistoryService(IUnitOfWork uow, IMapper mapper, TimeProvider timeProvider)
    {
        _uow = uow;
        _mapper = mapper;
        _timeProvider = timeProvider;
    }

    public async Task<SuccessResponse<IEnumerable<EmployeeEmploymentHistoryDto>>> GetAllAsync()
    {
        var histories = await _uow.Info.EmployeeEmploymentHistories.GetAllAsync();
        var dtos = _mapper.Map<IEnumerable<EmployeeEmploymentHistoryDto>>(histories);
        return SuccessResponse<IEnumerable<EmployeeEmploymentHistoryDto>>.Ok(dtos, EmployeeEmploymentHistoryMsg.RetrievedAll);
    }

    public async Task<SuccessResponse<EmployeeEmploymentHistoryDto>> GetByIdAsync(long id)
    {
        var history = await _uow.Info.EmployeeEmploymentHistories.GetByIdAsync(id);

        if (history == null)
            return SuccessResponse<EmployeeEmploymentHistoryDto>.Fail(EmployeeEmploymentHistoryMsg.NotFound(id), ErrorType.NotFound);

        var dto = _mapper.Map<EmployeeEmploymentHistoryDto>(history);
        return SuccessResponse<EmployeeEmploymentHistoryDto>.Ok(dto, EmployeeEmploymentHistoryMsg.Retrieved);
    }

    public async Task<SuccessResponse<IEnumerable<EmployeeEmploymentHistoryDto>>> GetByEmployeeIdAsync(long employeeId)
    {
        var histories = await _uow.Info.EmployeeEmploymentHistories.GetByEmployeeIdAsync(employeeId);
        var dtos = _mapper.Map<IEnumerable<EmployeeEmploymentHistoryDto>>(histories);
        return SuccessResponse<IEnumerable<EmployeeEmploymentHistoryDto>>.Ok(dtos, EmployeeEmploymentHistoryMsg.RetrievedAll);
    }
}
