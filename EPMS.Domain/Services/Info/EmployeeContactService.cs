using EPMS.Domain.Contracts;
using EPMS.Domain.Interface.IService.Info;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.EmployeeInfoDTOs;
using EPMS.Shared.Enums;
using Mapster;
using static EPMS.Shared.Constants.ServiceResponseMessages;
using EmployeeContact = EPMS.Domain.Entities.EmployeeInfo.EmployeeContact;

namespace EPMS.Domain.Services.Info;

public class EmployeeContactService : IEmployeeContactService
{
    private readonly IUnitOfWork _uow;

    public EmployeeContactService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<SuccessResponse<IEnumerable<EmployeeContactDto>>> GetAllAsync()
    {
        var contacts = await _uow.Info.EmployeeContacts.GetAllAsync();
        var dtos = contacts.Adapt<IEnumerable<EmployeeContactDto>>();
        return SuccessResponse<IEnumerable<EmployeeContactDto>>.Ok(dtos, EmployeeContactMsg.RetrievedAll);
    }

    public async Task<SuccessResponse<EmployeeContactDto>> GetByIdAsync(long id)
    {
        var contact = await _uow.Info.EmployeeContacts.GetByIdAsync(id);

        if (contact == null)
            return SuccessResponse<EmployeeContactDto>.Fail(EmployeeContactMsg.NotFound(id), ErrorType.NotFound);

        var dto = contact.Adapt<EmployeeContactDto>();
        return SuccessResponse<EmployeeContactDto>.Ok(dto, EmployeeContactMsg.Retrieved);
    }

    public async Task<SuccessResponse<EmployeeContactDto>> GetByEmployeeIdAsync(Guid employeePublicId)
    {
        var employee = await _uow.Info.EmployeeProfiles.GetByPublicIdAsync(employeePublicId);

        if (employee == null)
            return SuccessResponse<EmployeeContactDto>.Fail(EmployeeProfileMsg.NotFound(employeePublicId), ErrorType.NotFound);

        var contact = await _uow.Info.EmployeeContacts.GetByEmployeeIdAsync(employee.Id);

        if (contact == null)
            return SuccessResponse<EmployeeContactDto>.Fail(EmployeeContactMsg.NotFound(employeePublicId), ErrorType.NotFound);

        var dto = contact.Adapt<EmployeeContactDto>();
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

        // Update contact with provided details
        if (!string.IsNullOrWhiteSpace(dto.PhoneNo) ||
            !string.IsNullOrWhiteSpace(dto.ContactAddress))
        {
            contact.UpdatePrimaryContact(dto.PhoneNo, dto.ContactAddress);
        }

        if (!string.IsNullOrWhiteSpace(dto.PhoneNo) || !string.IsNullOrWhiteSpace(dto.PermanentPhoneNo) ||
            !string.IsNullOrWhiteSpace(dto.PresentPhoneNo) || !string.IsNullOrWhiteSpace(dto.InternalPhoneNo))
            contact.UpdatePhones(dto.PhoneNo, dto.PermanentPhoneNo, dto.PresentPhoneNo, dto.InternalPhoneNo);

        if (!string.IsNullOrWhiteSpace(dto.EmergencyMobileNo) || !string.IsNullOrWhiteSpace(dto.RelationWithEmergencyContact))
            contact.UpdateEmergencyContact(dto.EmergencyMobileNo, dto.RelationWithEmergencyContact);

        if (!string.IsNullOrWhiteSpace(dto.PermanentAddress))
        {
            contact.UpdatePermanentAddress(dto.PermanentAddress);
        }

        _uow.Info.EmployeeContacts.Add(contact);
        await _uow.CompleteAsync();

        return SuccessResponse<long>.Ok(contact.Id, EmployeeContactMsg.Created);
    }

    public async Task<SuccessResponse> UpdateAsync(long id, UpdateEmployeeContactDto dto)
    {
        var contact = await _uow.Info.EmployeeContacts.GetByIdAsync(id);

        if (contact == null)
            return SuccessResponse.Fail(EmployeeContactMsg.NotFound(id), ErrorType.NotFound);

        // Update contact fields using existing entity methods
        if (dto.PhoneNo != null || dto.ContactAddress != null)
            contact.UpdatePrimaryContact(dto.PhoneNo, dto.ContactAddress);

        if (dto.PermanentPhoneNo != null || dto.PresentPhoneNo != null || dto.InternalPhoneNo != null)
            contact.UpdatePhones(dto.PhoneNo, dto.PermanentPhoneNo, dto.PresentPhoneNo, dto.InternalPhoneNo);

        if (dto.EmergencyMobileNo != null || dto.RelationWithEmergencyContact != null)
            contact.UpdateEmergencyContact(dto.EmergencyMobileNo, dto.RelationWithEmergencyContact);

        if (dto.PermanentAddress != null)
            contact.UpdatePermanentAddress(dto.PermanentAddress);

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
