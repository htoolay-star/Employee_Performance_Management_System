using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Auth;
using EPMS.Domain.Entities.EmployeeInfo;
using EPMS.Domain.Interface.IService.App;
using EPMS.Domain.Interface.IService.Auth;
using EPMS.Domain.Interface.IService.Info;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.EmployeeInfoDTOs;
using EPMS.Shared.Enums;
using Mapster;
using static EPMS.Shared.Constants.ServiceResponseMessages;

namespace EPMS.Domain.Services.Info;

public class EmployeeContactService : IEmployeeContactService
{
    private readonly IUnitOfWork _uow;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ISystemSettingsService _settingsService;

    public EmployeeContactService(
        IUnitOfWork uow, 
        IPasswordHasher passwordHasher,
        ISystemSettingsService settingsService)
    {
        _uow = uow;
        _passwordHasher = passwordHasher;
        _settingsService = settingsService;
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

    public async Task<SuccessResponse<EmployeeContactDto>> GetByEmployeeIdAsync(long employeeId)
    {
        var contact = await _uow.Info.EmployeeContacts.GetByEmployeeIdAsync(employeeId);

        if (contact == null)
            return SuccessResponse<EmployeeContactDto>.Fail(EmployeeContactMsg.NotFound(employeeId), ErrorType.NotFound);

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

        // Create User if EmailAddress is provided and Employee has no User
        if (!string.IsNullOrWhiteSpace(dto.EmailAddress))
        {
            // Check if email already exists as a User
            var emailExists = await _uow.Auth.Users.ExistsAsync(dto.EmailAddress);
            if (emailExists)
                return SuccessResponse<long>.Fail(AuthMsg.EmailAlreadyRegistered, ErrorType.Conflict);

            // Check if EmployeeProfile already has a linked User
            if (profile.UserId != null)
                return SuccessResponse<long>.Fail("Employee already has a user account.", ErrorType.Conflict);

            // Create new User with default password and UserRole.User
            var defaultPassword = await _settingsService.GetDefaultPasswordAsync();
            var hashedPassword = _passwordHasher.Hash(defaultPassword);
            var newUser = new User(dto.EmailAddress, hashedPassword, UserRole.User);
            _uow.Auth.Users.Add(newUser);

            // Link User to EmployeeProfile
            profile.LinkUser(newUser.Id);
        }

        var contact = new EmployeeContact(dto.EmployeeId);
        
        // Update contact with provided details
        if (!string.IsNullOrWhiteSpace(dto.EmailAddress))
            contact.UpdatePrimaryContact(dto.EmailAddress, dto.PhoneNo, dto.ContactAddress);
        
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
        if (dto.EmailAddress != null || dto.PhoneNo != null || dto.ContactAddress != null)
            contact.UpdatePrimaryContact(dto.EmailAddress, dto.PhoneNo, dto.ContactAddress);

        if (dto.PermanentPhoneNo != null || dto.PresentPhoneNo != null || dto.InternalPhoneNo != null)
            contact.UpdatePhones(dto.PhoneNo, dto.PermanentPhoneNo, dto.PresentPhoneNo, dto.InternalPhoneNo);

        if (dto.EmergencyMobileNo != null || dto.RelationWithEmergencyContact != null)
            contact.UpdateEmergencyContact(dto.EmergencyMobileNo, dto.RelationWithEmergencyContact);

        if (dto.PermanentAddress != null)
            contact.UpdatePermanentAddress(dto.PermanentAddress);

        // Handle EmailAddress change - update existing User if Employee has one
        if (dto.EmailAddress != null)
        {
            var profile = await _uow.Info.EmployeeProfiles.GetByIdAsync(contact.EmployeeId);
            
            if (profile?.UserId != null)
            {
                var user = await _uow.Auth.Users.GetByIdAsync(profile.UserId.Value);
                
                if (user != null && user.Email != dto.EmailAddress)
                {
                    // Check if new email already exists as another user
                    var emailExists = await _uow.Auth.Users.ExistsAsync(dto.EmailAddress);
                    if (emailExists)
                        return SuccessResponse.Fail(AuthMsg.EmailAlreadyRegistered, ErrorType.Conflict);

                    // Update the existing User's email
                    user.UpdateEmail(dto.EmailAddress);
                }
            }
        }

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
