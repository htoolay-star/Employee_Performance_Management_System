using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Auth;
using EPMS.Domain.Entities.EmployeeInfo;
using EPMS.Domain.Interface.IService.App;
using EPMS.Domain.Interface.IService.Auth;
using EPMS.Domain.Interface.IService.Info;
using EPMS.Shared.Constants;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.EmployeeInfoDTOs;
using EPMS.Shared.Enums;
using Mapster;
using static EPMS.Shared.Constants.ServiceResponseMessages;

namespace EPMS.Domain.Services.Info;

public class EmployeeProfileService : IEmployeeProfileService
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentEmployeeContextService _currentEmployee;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ISystemSettingsService _settingsService;
    private readonly ICacheService _cacheService;

    public EmployeeProfileService(
        IUnitOfWork uow,
        ICurrentEmployeeContextService currentEmployee,
        IPasswordHasher passwordHasher,
        ISystemSettingsService settingsService,
        ICacheService cacheService)
    {
        _uow = uow;
        _currentEmployee = currentEmployee;
        _cacheService = cacheService;
        _passwordHasher = passwordHasher;
        _settingsService = settingsService;
    }

    public async Task<SuccessResponse<IEnumerable<EmployeeProfileDto>>> GetAllAsync()
    {
        var profiles = await _uow.Info.EmployeeProfiles.GetAllAsync();
        var dtos = profiles.Adapt<IEnumerable<EmployeeProfileDto>>();
        return SuccessResponse<IEnumerable<EmployeeProfileDto>>.Ok(dtos, EmployeeProfileMsg.RetrievedAll);
    }

    public async Task<SuccessResponse<EmployeeProfileDto>> GetByIdAsync(long id)
    {
        var profile = await _uow.Info.EmployeeProfiles.GetByIdAsync(id);

        if (profile == null)
            return SuccessResponse<EmployeeProfileDto>.Fail(EmployeeProfileMsg.NotFound(id), ErrorType.NotFound);

        var dto = profile.Adapt<EmployeeProfileDto>();
        return SuccessResponse<EmployeeProfileDto>.Ok(dto, EmployeeProfileMsg.Retrieved);
    }

    public async Task<SuccessResponse<EmployeeProfileDto>> GetByPublicIdAsync(Guid publicId)
    {
        var profile = await _uow.Info.EmployeeProfiles.GetByPublicIdAsync(publicId);

        if (profile == null)
            return SuccessResponse<EmployeeProfileDto>.Fail(EmployeeProfileMsg.NotFound(publicId), ErrorType.NotFound);

        var dto = profile.Adapt<EmployeeProfileDto>();
        return SuccessResponse<EmployeeProfileDto>.Ok(dto, EmployeeProfileMsg.Retrieved);
    }

    public async Task<SuccessResponse<long>> CreateAsync(CreateEmployeeProfileDto dto)
    {
        // Check for duplicate StaffNo
        var existing = await _uow.Info.EmployeeProfiles.GetByStaffNoAsync(dto.StaffNo);
        if (existing != null)
            return SuccessResponse<long>.Fail(string.Format(EmployeeProfileMsg.DuplicateStaffNo, dto.StaffNo), ErrorType.Conflict);

        // Check for duplicate UserId if provided
        if (dto.UserId.HasValue)
        {
            var existingUser = await _uow.Info.EmployeeProfiles.GetByUserIdAsync(dto.UserId.Value);
            if (existingUser != null)
                return SuccessResponse<long>.Fail(string.Format(EmployeeProfileMsg.DuplicateUserId, dto.UserId.Value), ErrorType.Conflict);
        }

        // Check for duplicate EmailAddress
        if (!string.IsNullOrWhiteSpace(dto.EmailAddress))
        {
            var emailExists = await _uow.Info.EmployeeProfiles.ExistsByEmailAsync(dto.EmailAddress);
            if (emailExists)
                return SuccessResponse<long>.Fail(string.Format(EmployeeProfileMsg.DuplicateEmail, dto.EmailAddress), ErrorType.Conflict);
        }

        var profile = new EmployeeProfile(dto.UserId, dto.StaffNo, dto.StaffName, dto.EmailAddress);
        
        // Set additional properties using entity methods
        if (!string.IsNullOrEmpty(dto.OtherName)) profile.UpdateOtherName(dto.OtherName);
        if (!string.IsNullOrEmpty(dto.NRCNo)) profile.UpdateNRCNo(dto.NRCNo);
        if (!string.IsNullOrEmpty(dto.Gender)) profile.UpdateDemographics(dto.Gender, dto.DateOfBirth, dto.Nationality);

        _uow.Info.EmployeeProfiles.Add(profile);
        await _uow.CompleteAsync();

        await _cacheService.RemoveAsync(CacheKeys.Hr.EmployeeLookups());

        // Create User if EmailAddress is provided and Employee has no linked User
        if (!string.IsNullOrWhiteSpace(dto.EmailAddress))
        {
            var emailExists = await _uow.Auth.Users.ExistsAsync(dto.EmailAddress);
            if (emailExists)
                return SuccessResponse<long>.Fail(AuthMsg.EmailAlreadyRegistered, ErrorType.Conflict);

            var defaultPassword = await _settingsService.GetDefaultPasswordAsync();
            var hashedPassword = _passwordHasher.Hash(defaultPassword);
            var newUser = new User(dto.EmailAddress, hashedPassword, UserRole.User);
            _uow.Auth.Users.Add(newUser);

            profile.LinkUser(newUser.Id);
            await _uow.CompleteAsync();
        }
        
        return SuccessResponse<long>.Ok(profile.Id, EmployeeProfileMsg.Created);
    }

    public async Task<SuccessResponse<long>> CreateFullAsync(CreateFullEmployeeDto dto)
    {
        await _uow.BeginTransactionAsync();
        try
        {
            // 1. Create Profile (handles dedup checks + User creation internally)
            var profileResult = await CreateAsync(dto.Profile);
            if (!profileResult.Success)
            {
                await _uow.RollbackAsync();
                return profileResult;
            }
            var employeeId = profileResult.Data!;

            // 2. Create Employment (if provided)
            if (dto.Employment != null)
            {
                var emp = dto.Employment;
                var employment = new EmployeeEmployment(
                    employeeId, emp.DepartmentId, emp.ParentDepartmentId,
                    emp.PositionId, emp.EmploymentStatus);
                if (!string.IsNullOrEmpty(emp.StaffType))
                    employment.UpdateDetails(emp.DepartmentId, emp.ParentDepartmentId, emp.PositionId,
                        emp.TeamId, emp.DirectManagerId, emp.EmploymentStatus,
                        emp.StaffType, emp.ProbationMonth, emp.Shift, emp.FingerPrintId, emp.MobileAttendance);
                if (!string.IsNullOrEmpty(emp.ProductProject))
                    employment.AssignProject(emp.ProductProject);
                _uow.Info.EmployeeEmployments.Add(employment);
            }

            // 3. Create Contact (if provided)
            if (dto.Contact != null)
            {
                var con = dto.Contact;
                var contact = new EmployeeContact(employeeId);
                if (!string.IsNullOrEmpty(con.PhoneNo) || !string.IsNullOrEmpty(con.ContactAddress))
                    contact.UpdatePrimaryContact(con.PhoneNo, con.ContactAddress);
                if (!string.IsNullOrEmpty(con.EmergencyMobileNo) || !string.IsNullOrEmpty(con.RelationWithEmergencyContact))
                    contact.UpdateEmergencyContact(con.EmergencyMobileNo, con.RelationWithEmergencyContact);
                if (!string.IsNullOrEmpty(con.PermanentAddress))
                    contact.UpdatePermanentAddress(con.PermanentAddress);
                _uow.Info.EmployeeContacts.Add(contact);
            }

            // 4. Create Family (if provided)
            if (dto.Family != null)
            {
                var fam = dto.Family;
                var family = new EmployeeFamilyInfo(employeeId);
                if (!string.IsNullOrEmpty(fam.MaritalStatus))
                    family.UpdateMaritalStatus(fam.MaritalStatus, fam.SpouseName, fam.SpouseNRCNo, fam.SpouseOccupation);
                if (!string.IsNullOrEmpty(fam.FatherName))
                    family.UpdateFatherDetails(fam.FatherName, fam.FatherNRCNo, fam.FatherOccupation);
                _uow.Info.EmployeeFamilyInfos.Add(family);
            }

            // 5. Create Payroll (if provided)
            if (dto.Payroll != null)
            {
                var pay = dto.Payroll;
                var payroll = new EmployeePayrollInfo(employeeId, pay.Salary, pay.Currency);
                if (!string.IsNullOrEmpty(pay.TaxStatus) || !string.IsNullOrEmpty(pay.TaxNo))
                    payroll.UpdateTaxInfo(pay.TaxStatus, pay.TaxNo);
                if (!string.IsNullOrEmpty(pay.SSBStatus) || !string.IsNullOrEmpty(pay.SSCBNo))
                    payroll.UpdateSSBInfo(pay.SSBStatus, pay.SSCBNo);
                if (pay.ComplianceEarnedPoints.HasValue || pay.ComplianceBalancePoints.HasValue)
                    payroll.UpdateCompliancePoints(pay.ComplianceEarnedPoints, pay.ComplianceBalancePoints);
                _uow.Info.EmployeePayrollInfos.Add(payroll);
            }

            await _cacheService.RemoveAsync(CacheKeys.Hr.EmployeeLookups());
            await _uow.CommitAsync();
            return SuccessResponse<long>.Ok(employeeId, EmployeeProfileMsg.Created);
        }
        catch
        {
            await _uow.RollbackAsync();
            throw;
        }
    }

    public async Task<SuccessResponse> UpdateAsync(long id, UpdateEmployeeProfileDto dto)
    {
        var profile = await _uow.Info.EmployeeProfiles.GetByIdAsync(id);

        if (profile == null)
            return SuccessResponse.Fail(EmployeeProfileMsg.NotFound(id), ErrorType.NotFound);

        profile.UpdateStaffName(dto.StaffName);
        if (dto.OtherName != null) profile.UpdateOtherName(dto.OtherName);
        
        profile.UpdateDemographics(dto.Gender, dto.DateOfBirth, dto.Nationality);
        
        // Check for duplicate EmailAddress (excluding current profile)
        if (dto.EmailAddress != null && dto.EmailAddress != profile.EmailAddress)
        {
            var emailExists = await _uow.Info.EmployeeProfiles.ExistsByEmailAsync(dto.EmailAddress, id);
            if (emailExists)
                return SuccessResponse.Fail(string.Format(EmployeeProfileMsg.DuplicateEmail, dto.EmailAddress), ErrorType.Conflict);
        }

        if (dto.EmailAddress != null) profile.UpdateEmail(dto.EmailAddress);
        
        // Sync email change to linked User account
        if (dto.EmailAddress != null && profile.UserId != null)
        {
            var user = await _uow.Auth.Users.GetByIdAsync(profile.UserId.Value);
            if (user != null && user.Email != dto.EmailAddress)
            {
                var emailExists = await _uow.Auth.Users.ExistsAsync(dto.EmailAddress);
                if (emailExists)
                    return SuccessResponse.Fail(AuthMsg.EmailAlreadyRegistered, ErrorType.Conflict);

                user.UpdateEmail(dto.EmailAddress);
            }
        }

        if (!string.IsNullOrEmpty(dto.WorkPermitNo))
            profile.UpdateWorkPermit(dto.WorkPermitNo, dto.WorkPermitValidDate, dto.WorkPermitExpireDate);
        
        if (!string.IsNullOrEmpty(dto.ProfilePictureUrl))
            profile.UpdateProfilePicture(dto.ProfilePictureUrl, dto.ProfileThumbnailUrl);
        
        if (!string.IsNullOrEmpty(dto.AdditionalData))
            profile.UpdateAdditionalData(dto.AdditionalData);

        await _uow.CompleteAsync();
        await _cacheService.RemoveAsync(CacheKeys.Hr.EmployeeLookups());
        return SuccessResponse.Ok(EmployeeProfileMsg.Updated);
    }

    public async Task<SuccessResponse> DeleteAsync(long id)
    {
        var profile = await _uow.Info.EmployeeProfiles.GetByIdAsync(id);

        if (profile == null)
            return SuccessResponse.Fail(EmployeeProfileMsg.NotFound(id), ErrorType.NotFound);

        _uow.Info.EmployeeProfiles.Delete(profile);
        await _uow.CompleteAsync();
        await _cacheService.RemoveAsync(CacheKeys.Hr.EmployeeLookups());
        return SuccessResponse.Ok(EmployeeProfileMsg.Deleted);
    }

    public async Task<SuccessResponse<EmployeeProfileDto>> GetByStaffNoAsync(string staffNo)
    {
        var profile = await _uow.Info.EmployeeProfiles.GetByStaffNoAsync(staffNo);

        if (profile == null)
            return SuccessResponse<EmployeeProfileDto>.Fail(EmployeeProfileMsg.NotFound(0), ErrorType.NotFound);

        var dto = profile.Adapt<EmployeeProfileDto>();
        return SuccessResponse<EmployeeProfileDto>.Ok(dto, EmployeeProfileMsg.Retrieved);
    }

    public async Task<SuccessResponse<EmployeeProfileDto>> GetByUserIdAsync(long userId)
    {
        var profile = await _uow.Info.EmployeeProfiles.GetByUserIdAsync(userId);

        if (profile == null)
            return SuccessResponse<EmployeeProfileDto>.Fail(EmployeeProfileMsg.NotFound(0), ErrorType.NotFound);

        var dto = profile.Adapt<EmployeeProfileDto>();
        return SuccessResponse<EmployeeProfileDto>.Ok(dto, EmployeeProfileMsg.Retrieved);
    }

    public async Task<SuccessResponse<IEnumerable<EmployeeLookupDto>>> GetLookupAsync()
    {
        var dtos = await _cacheService.GetOrCreateAsync(
            CacheKeys.Hr.EmployeeLookups(),
            async () => await _uow.Info.EmployeeProfiles.GetLookupDtoAsync(),
            TimeSpan.FromHours(1)
        );
        return SuccessResponse<IEnumerable<EmployeeLookupDto>>.Ok(dtos ?? [], EmployeeProfileMsg.RetrievedAll);
    }

    public async Task<SuccessResponse<PaginatedResponse<EmployeeProfileGridItemDto>>> GetPagedAsync(EPMS.Shared.Features.EmployeeProfiles.EmployeeProfileQueryParameters parameters)
    {
        var entitySortColumn = GetMappedSortColumn(parameters.OrderBy);
        var (dtos, totalCount) = await _uow.Info.EmployeeProfiles.GetPagedDtoAsync(parameters, entitySortColumn);

        var response = new PaginatedResponse<EmployeeProfileGridItemDto>
        {
            Items = dtos.ToList(),
            TotalCount = totalCount,
            PageNumber = parameters.PageNumber,
            PageSize = parameters.PageSize
        };

        return SuccessResponse<PaginatedResponse<EmployeeProfileGridItemDto>>.Ok(response, EmployeeProfileMsg.RetrievedAll);
    }

    private static string GetMappedSortColumn(string? orderBy)
    {
        if (string.IsNullOrWhiteSpace(orderBy)) return "StaffName";

        var columnMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "StaffName", "StaffName" },
            { "StaffNo", "StaffNo" }
        };

        return columnMap.TryGetValue(orderBy, out var mappedColumn) ? mappedColumn : "StaffName";
    }
}
