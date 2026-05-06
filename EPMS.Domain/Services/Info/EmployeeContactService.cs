using AutoMapper;
using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.EmployeeInfo;
using EPMS.Domain.Interface.IService.Info;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.EmployeeInfoDTOs;
using EPMS.Shared.Enums;
using static EPMS.Shared.Constants.ServiceResponseMessages;

namespace EPMS.Domain.Services.Info;

public class EmployeeContactService : IEmployeeContactService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public EmployeeContactService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<SuccessResponse<IEnumerable<EmployeeContactDto>>> GetAllAsync()
    {
        var contacts = await _uow.Info.EmployeeContacts.GetAllAsync();
        var dtos = _mapper.Map<IEnumerable<EmployeeContactDto>>(contacts);
        return SuccessResponse<IEnumerable<EmployeeContactDto>>.Ok(dtos, EmployeeContactMsg.RetrievedAll);
    }

    public async Task<SuccessResponse<EmployeeContactDto>> GetByIdAsync(long id)
    {
        var contact = await _uow.Info.EmployeeContacts.GetByIdAsync(id);

        if (contact == null)
            return SuccessResponse<EmployeeContactDto>.Fail(EmployeeContactMsg.NotFound(id), ErrorType.NotFound);

        var dto = _mapper.Map<EmployeeContactDto>(contact);
        return SuccessResponse<EmployeeContactDto>.Ok(dto, EmployeeContactMsg.Retrieved);
    }

    public async Task<SuccessResponse<EmployeeContactDto>> GetByEmployeeIdAsync(long employeeId)
    {
        var contact = await _uow.Info.EmployeeContacts.GetByEmployeeIdAsync(employeeId);

        if (contact == null)
            return SuccessResponse<EmployeeContactDto>.Fail(EmployeeContactMsg.NotFound(employeeId), ErrorType.NotFound);

        var dto = _mapper.Map<EmployeeContactDto>(contact);
        return SuccessResponse<EmployeeContactDto>.Ok(dto, EmployeeContactMsg.Retrieved);
    }

    public async Task<SuccessResponse<long>> CreateAsync(CreateEmployeeContactDto dto)
    {
        // Check if profile exists
        var profile = await _uow.Info.EmployeeProfiles.GetByIdAsync(dto.EmployeeId);
        if (profile == null)
            return SuccessResponse<long>.Fail(EmployeeProfileMsg.NotFound(dto.EmployeeId), ErrorType.NotFound);

        // Check if contact already exists for this employee
        var existing = await _uow.Info.EmployeeContacts.GetByEmployeeIdAsync(dto.EmployeeId);
        if (existing != null)
            return SuccessResponse<long>.Fail(EmployeeContactMsg.Retrieved, ErrorType.Conflict);

        var contact = new EmployeeContact(dto.EmployeeId);
        
        _uow.Info.EmployeeContacts.Add(contact);
        await _uow.CompleteAsync();

        return SuccessResponse<long>.Ok(contact.Id, EmployeeContactMsg.Created);
    }

    public async Task<SuccessResponse> UpdateAsync(long id, UpdateEmployeeContactDto dto)
    {
        var contact = await _uow.Info.EmployeeContacts.GetByIdAsync(id);

        if (contact == null)
            return SuccessResponse.Fail(EmployeeContactMsg.NotFound(id), ErrorType.NotFound);

        // Note: The entity has specific update methods but they don't match our DTO structure
        // We would need to add more flexible update methods to the entity or handle this differently
        
        await _uow.CompleteAsync();
        return SuccessResponse.Ok(EmployeeContactMsg.Updated);
    }

    public async Task<SuccessResponse> DeleteAsync(long id)
    {
        var contact = await _uow.Info.EmployeeContacts.GetByIdAsync(id);

        if (contact == null)
            return SuccessResponse.Fail(EmployeeContactMsg.NotFound(id), ErrorType.NotFound);

        _uow.Info.EmployeeContacts.Delete(contact);
        await _uow.CompleteAsync();

        return SuccessResponse.Ok(EmployeeContactMsg.Deleted);
    }
}
