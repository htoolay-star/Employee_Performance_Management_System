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
        var saIds = await GetSystemAdminEmployeeIdsAsync();
        var dtos = profiles.Where(p => !saIds.Contains(p.Id)).Adapt<IEnumerable<EmployeeProfileDto>>();
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

    public async Task<SuccessResponse<long>> CreateAsync(CreateEmployeeProfileDto dto, string? preHashedPassword = null)
    {
        // Check for duplicate StaffNo
        var existing = await _uow.Info.EmployeeProfiles.GetByStaffNoAsync(dto.StaffNo);
        if (existing != null)
            return SuccessResponse<long>.Fail(string.Format(EmployeeProfileMsg.DuplicateStaffNo, dto.StaffNo), ErrorType.Conflict);

        // Check for duplicate UserId if provided
        if (dto.UserId.HasValue)
        {
            var user = await _uow.Auth.Users.GetByIdAsync(dto.UserId.Value);
            if (user == null)
                return SuccessResponse<long>.Fail(EmployeeProfileMsg.UserNotFound, ErrorType.NotFound);

            var existingProfile = await _uow.Info.EmployeeProfiles.GetByUserIdAsync(dto.UserId.Value);
            if (existingProfile != null)
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

            if (preHashedPassword == null)
            {
                var defaultPassword = await _settingsService.GetDefaultPasswordAsync();
                preHashedPassword = _passwordHasher.Hash(defaultPassword);
            }
            var newUser = new User(dto.EmailAddress, preHashedPassword, UserRole.User);
            _uow.Auth.Users.Add(newUser);
            await _uow.CompleteAsync();

            profile.LinkUser(newUser.Id);
            await _uow.CompleteAsync();
        }
        
        return SuccessResponse<long>.Ok(profile.Id, EmployeeProfileMsg.Created);
    }

    public async Task<SuccessResponse<long>> CreateFullAsync(CreateFullEmployeeDto dto, string? preHashedPassword = null)
    {
        await _uow.BeginTransactionAsync();
        try
        {
            // 1. Create Profile (handles dedup checks + User creation internally)
            var profileResult = await CreateAsync(dto.Profile, preHashedPassword);
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
            async () =>
            {
                var all = await _uow.Info.EmployeeProfiles.GetLookupDtoAsync();
                var saIds = await GetSystemAdminEmployeeIdsAsync();
                return all?.Where(d => !saIds.Contains(d.Id)).ToList() ?? [];
            },
            TimeSpan.FromHours(1)
        );
        return SuccessResponse<IEnumerable<EmployeeLookupDto>>.Ok(dtos ?? [], EmployeeProfileMsg.RetrievedAll);
    }

    public async Task<SuccessResponse<PaginatedResponse<EmployeeProfileGridItemDto>>> GetPagedAsync(EPMS.Shared.Features.EmployeeProfiles.EmployeeProfileQueryParameters parameters)
    {
        var entitySortColumn = GetMappedSortColumn(parameters.OrderBy);
        var saIds = await GetSystemAdminEmployeeIdsAsync();
        var (dtos, totalCount) = await _uow.Info.EmployeeProfiles.GetPagedDtoAsync(parameters, entitySortColumn, excludeEmployeeIds: saIds);

        var response = new PaginatedResponse<EmployeeProfileGridItemDto>
        {
            Items = dtos.ToList(),
            TotalCount = totalCount,
            PageNumber = parameters.PageNumber,
            PageSize = parameters.PageSize
        };

        return SuccessResponse<PaginatedResponse<EmployeeProfileGridItemDto>>.Ok(response, EmployeeProfileMsg.RetrievedAll);
    }

    private async Task<HashSet<long>> GetSystemAdminEmployeeIdsAsync()
    {
        var saUsers = await _uow.Auth.Users
            .FindAllAsync(u => u.RoleId == (long)UserRole.SystemAdmin && !u.IsDeleted,
                          includes: u => u.Profile);
        return saUsers
            .Where(u => u.Profile != null)
            .Select(u => u.Profile!.Id)
            .ToHashSet();
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

    public async Task<SuccessResponse<IEnumerable<EmployeeFullImportRow>>> GetFullExportAsync()
    {
        var profiles = (await _uow.Info.EmployeeProfiles.GetAllAsync()).ToList();
        var employeeIds = profiles.Select(p => p.Id).ToList();

        var employments = (await _uow.Info.EmployeeEmployments.FindAllAsync(e => employeeIds.Contains(e.EmployeeId))).ToDictionary(e => e.EmployeeId);
        var contacts = (await _uow.Info.EmployeeContacts.FindAllAsync(c => employeeIds.Contains(c.EmployeeId))).ToDictionary(c => c.EmployeeId);
        var families = (await _uow.Info.EmployeeFamilyInfos.FindAllAsync(f => employeeIds.Contains(f.EmployeeId))).ToDictionary(f => f.EmployeeId);
        var payrolls = (await _uow.Info.EmployeePayrollInfos.FindAllAsync(p => employeeIds.Contains(p.EmployeeId))).ToDictionary(p => p.EmployeeId);

        var deptDict = (await _uow.HR.Departments.GetAllAsync()).ToDictionary(d => d.Id);
        var posDict = (await _uow.HR.Positions.GetAllAsync()).ToDictionary(p => p.Id);
        var teamDict = (await _uow.HR.Teams.GetAllAsync()).ToDictionary(t => t.Id);
        var profileDict = profiles.ToDictionary(p => p.Id);

        var rows = profiles.Select(p =>
        {
            employments.TryGetValue(p.Id, out var emp);
            contacts.TryGetValue(p.Id, out var con);
            families.TryGetValue(p.Id, out var fam);
            payrolls.TryGetValue(p.Id, out var pay);

            var dept = emp != null ? deptDict.GetValueOrDefault(emp.DepartmentId) : null;
            var parentDept = emp != null ? deptDict.GetValueOrDefault(emp.ParentDepartmentId) : null;
            var pos = emp != null ? posDict.GetValueOrDefault(emp.PositionId) : null;
            var team = emp != null && emp.TeamId.HasValue ? teamDict.GetValueOrDefault(emp.TeamId.Value) : null;
            var mgr = emp != null && emp.DirectManagerId.HasValue ? profileDict.GetValueOrDefault(emp.DirectManagerId.Value) : null;

            return new EmployeeFullImportRow
            {
                StaffNo = p.StaffNo,
                StaffName = p.StaffName,
                OtherName = p.OtherName,
                Gender = p.Gender,
                NRCNo = p.NRCNo,
                Race = p.Race,
                Religion = p.Religion,
                Nationality = p.Nationality,
                BirthPlace = p.BirthPlace,
                EmailAddress = p.EmailAddress,
                DateOfBirth = p.DateOfBirth,
                PassportNo = p.PassportNo,
                PassportExpireDate = p.PassportExpireDate,
                LabourRegistrationNo = p.LabourRegistrationNo,
                WorkPermitNo = p.WorkPermitNo,
                WorkPermitValidDate = p.WorkPermitValidDate,
                WorkPermitExpireDate = p.WorkPermitExpireDate,

                EmploymentStatus = emp?.EmploymentStatus,
                StaffType = emp?.StaffType,
                ProbationMonth = emp?.ProbationMonth,
                Shift = emp?.Shift,
                DateOfAppointment = emp?.DateOfAppointment,
                DateOfConfirmation = emp?.DateOfConfirmation,
                DateOfPromotion = emp?.DateOfPromotion,
                DateOfTermination = emp?.DateOfTermination,
                DateOfTransfer = emp?.DateOfTransfer,
                DateOfDemotion = emp?.DateOfDemotion,
                DateOfTitleChange = emp?.DateOfTitleChange,
                DateOfIncrement = emp?.DateOfIncrement,
                DepartmentName = dept?.Name,
                ParentDepartmentName = parentDept?.Name,
                TeamName = team?.Name,
                PositionName = pos?.Name,
                DirectManagerStaffNo = mgr?.StaffNo,
                ProductProject = emp?.ProductProject,
                FingerPrintId = emp?.FingerPrintId,
                MobileAttendance = emp?.MobileAttendance ?? false,

                ContactAddress = con?.ContactAddress,
                PermanentAddress = con?.PermanentAddress,
                PhoneNo = con?.PhoneNo,
                PermanentPhoneNo = con?.PermanentPhoneNo,
                PresentPhoneNo = con?.PresentPhoneNo,
                InternalPhoneNo = con?.InternalPhoneNo,
                EmergencyMobileNo = con?.EmergencyMobileNo,
                RelationWithEmergencyContact = con?.RelationWithEmergencyContact,

                MaritalStatus = fam?.MaritalStatus,
                SpouseName = fam?.SpouseName,
                SpouseNRCNo = fam?.SpouseNRCNo,
                SpouseOccupation = fam?.SpouseOccupation,
                FatherName = fam?.FatherName,
                FatherNRCNo = fam?.FatherNRCNo,
                FatherOccupation = fam?.FatherOccupation,

                Salary = pay?.Salary,
                Currency = pay?.Currency,
                PayType = pay?.PayType,
                DateOfPayTypeChanged = pay?.DateOfPayTypeChanged,
                DateOfSalaryChanged = pay?.DateOfSalaryChanged,
                DateOfCurrencyChange = pay?.DateOfCurrencyChange,
                CostAllocate = pay?.CostAllocate,
                PayByBacklog = pay?.PayByBacklog,
                TaxStatus = pay?.TaxStatus,
                TaxNo = pay?.TaxNo,
                SSBStatus = pay?.SSBStatus,
                SSCBNo = pay?.SSCBNo,
                ComplianceEarnedPoints = pay?.ComplianceEarnedPoints,
                ComplianceBalancePoints = pay?.ComplianceBalancePoints
            };
        }).ToList();

        return SuccessResponse<IEnumerable<EmployeeFullImportRow>>.Ok(rows, EmployeeProfileMsg.RetrievedAll);
    }

    public async Task<SuccessResponse<ImportResult>> ImportFullEmployeesAsync(List<EmployeeFullImportRow> rows)
    {
        var errors = new List<string>();
        var successCount = 0;

        var departments = (await _uow.HR.Departments.GetAllAsync()).ToDictionary(d => d.Name, StringComparer.OrdinalIgnoreCase);
        var positions = (await _uow.HR.Positions.GetAllAsync()).ToDictionary(p => p.Name, StringComparer.OrdinalIgnoreCase);
        var teams = (await _uow.HR.Teams.GetAllAsync()).ToDictionary(t => t.Name, StringComparer.OrdinalIgnoreCase);

        var allProfiles = (await _uow.Info.EmployeeProfiles.GetAllAsync()).ToList();
        var existingStaffNos = new HashSet<string>(allProfiles.Select(p => p.StaffNo), StringComparer.OrdinalIgnoreCase);
        var existingEmails = new HashSet<string>(allProfiles.Where(p => p.EmailAddress != null).Select(p => p.EmailAddress!), StringComparer.OrdinalIgnoreCase);
        var profileByStaffNo = allProfiles.ToDictionary(p => p.StaffNo, StringComparer.OrdinalIgnoreCase);

        var defaultPassword = await _settingsService.GetDefaultPasswordAsync();
        var preHashedPassword = _passwordHasher.Hash(defaultPassword);

        foreach (var row in rows)
        {
            try
            {
                var rowErrors = new List<string>();
            var rowNum = rows.IndexOf(row) + 1;

                if (string.IsNullOrWhiteSpace(row.StaffNo))
                { rowErrors.Add($"Row {rowNum}: StaffNo is required."); }
                else if (existingStaffNos.Contains(row.StaffNo))
                { rowErrors.Add($"Row {rowNum}: StaffNo '{row.StaffNo}' already exists."); }

                if (!string.IsNullOrWhiteSpace(row.EmailAddress) && existingEmails.Contains(row.EmailAddress))
                { rowErrors.Add($"Row {rowNum}: Email '{row.EmailAddress}' already exists."); }

                if (string.IsNullOrWhiteSpace(row.EmploymentStatus))
                    rowErrors.Add($"Row {rowNum}: EmploymentStatus is required.");

                if (!string.IsNullOrWhiteSpace(row.DepartmentName))
                {
                    if (!departments.TryGetValue(row.DepartmentName, out var dept))
                        rowErrors.Add($"Row {rowNum}: Department '{row.DepartmentName}' not found.");
                }

                if (!string.IsNullOrWhiteSpace(row.PositionName))
                {
                    if (!positions.TryGetValue(row.PositionName, out var pos))
                        rowErrors.Add($"Row {rowNum}: Position '{row.PositionName}' not found.");
                }

                if (!string.IsNullOrWhiteSpace(row.TeamName))
                {
                    if (!teams.TryGetValue(row.TeamName, out var team))
                        rowErrors.Add($"Row {rowNum}: Team '{row.TeamName}' not found.");
                }

                if (rowErrors.Count > 0)
                {
                    errors.AddRange(rowErrors);
                    continue;
                }

                var deptId = departments.GetValueOrDefault(row.DepartmentName ?? "")?.Id ?? 0;
                var parentDeptId = deptId;
                if (!string.IsNullOrWhiteSpace(row.ParentDepartmentName) && departments.TryGetValue(row.ParentDepartmentName, out var parentDept))
                    parentDeptId = parentDept.Id;

                var posId = positions.GetValueOrDefault(row.PositionName ?? "")?.Id ?? 0;
                var teamId = teams.GetValueOrDefault(row.TeamName ?? "")?.Id;

                long? managerId = null;
                if (!string.IsNullOrWhiteSpace(row.DirectManagerStaffNo) &&
                    profileByStaffNo.TryGetValue(row.DirectManagerStaffNo, out var mgr))
                {
                    managerId = mgr.Id;
                }

                var dto = new CreateFullEmployeeDto
                {
                    Profile = new CreateEmployeeProfileDto
                    {
                        StaffNo = row.StaffNo,
                        StaffName = row.StaffName,
                        OtherName = row.OtherName,
                        Gender = row.Gender,
                        NRCNo = row.NRCNo,
                        Race = row.Race,
                        Religion = row.Religion,
                        Nationality = row.Nationality,
                        BirthPlace = row.BirthPlace,
                        EmailAddress = row.EmailAddress,
                        DateOfBirth = row.DateOfBirth,
                        PassportNo = row.PassportNo,
                        PassportExpireDate = row.PassportExpireDate,
                        LabourRegistrationNo = row.LabourRegistrationNo,
                        WorkPermitNo = row.WorkPermitNo,
                        WorkPermitValidDate = row.WorkPermitValidDate,
                        WorkPermitExpireDate = row.WorkPermitExpireDate
                    },
                    Employment = new CreateEmployeeEmploymentDto
                    {
                        DepartmentId = deptId,
                        ParentDepartmentId = parentDeptId,
                        PositionId = posId,
                        TeamId = teamId,
                        DirectManagerId = managerId,
                        EmploymentStatus = row.EmploymentStatus ?? EmploymentStatuses.Pending,
                        StaffType = row.StaffType,
                        ProbationMonth = row.ProbationMonth,
                        Shift = row.Shift,
                        DateOfAppointment = row.DateOfAppointment,
                        FingerPrintId = row.FingerPrintId,
                        MobileAttendance = row.MobileAttendance,
                        ProductProject = row.ProductProject
                    },
                    Contact = new CreateEmployeeContactDto
                    {
                        ContactAddress = row.ContactAddress,
                        PermanentAddress = row.PermanentAddress,
                        PhoneNo = row.PhoneNo,
                        PermanentPhoneNo = row.PermanentPhoneNo,
                        PresentPhoneNo = row.PresentPhoneNo,
                        InternalPhoneNo = row.InternalPhoneNo,
                        EmergencyMobileNo = row.EmergencyMobileNo,
                        RelationWithEmergencyContact = row.RelationWithEmergencyContact
                    },
                    Family = new CreateEmployeeFamilyInfoDto
                    {
                        MaritalStatus = row.MaritalStatus,
                        SpouseName = row.SpouseName,
                        SpouseNRCNo = row.SpouseNRCNo,
                        SpouseOccupation = row.SpouseOccupation,
                        FatherName = row.FatherName,
                        FatherNRCNo = row.FatherNRCNo,
                        FatherOccupation = row.FatherOccupation
                    },
                    Payroll = row.Salary.HasValue ? new CreateEmployeePayrollInfoDto
                    {
                        Salary = row.Salary.Value,
                        Currency = row.Currency ?? Currency.USD,
                        PayType = row.PayType,
                        CostAllocate = row.CostAllocate,
                        PayByBacklog = row.PayByBacklog,
                        TaxStatus = row.TaxStatus,
                        TaxNo = row.TaxNo,
                        SSBStatus = row.SSBStatus,
                        SSCBNo = row.SSCBNo,
                        ComplianceEarnedPoints = row.ComplianceEarnedPoints,
                        ComplianceBalancePoints = row.ComplianceBalancePoints
                    } : null
                };

                var result = await CreateFullAsync(dto, preHashedPassword);
                if (result.Success)
                    successCount++;
                else
                    errors.Add($"Row {rowNum}: {result.Message}");
            }
            catch (Exception ex)
            {
                errors.Add($"Row {rows.IndexOf(row) + 2}: {ex.Message}");
            }
        }

        var importResult = new ImportResult
        {
            TotalRows = rows.Count,
            SuccessCount = successCount,
            ErrorCount = errors.Count,
            Errors = errors
        };

        return SuccessResponse<ImportResult>.Ok(importResult,
            $"{successCount} employees created, {errors.Count} errors.");
    }

    public async Task<SuccessResponse<ImportPreviewResult>> ImportPreviewAsync(List<EmployeeFullImportRow> rows)
    {
        var departments = (await _uow.HR.Departments.GetAllAsync())
            .ToDictionary(d => d.Name, StringComparer.OrdinalIgnoreCase);
        var positions = (await _uow.HR.Positions.GetAllAsync())
            .ToDictionary(p => p.Name, StringComparer.OrdinalIgnoreCase);
        var teams = (await _uow.HR.Teams.GetAllAsync())
            .ToDictionary(t => t.Name, StringComparer.OrdinalIgnoreCase);

        var allProfiles = (await _uow.Info.EmployeeProfiles.GetAllAsync()).ToList();
        var existingStaffNos = new HashSet<string>(allProfiles.Select(p => p.StaffNo), StringComparer.OrdinalIgnoreCase);
        var existingEmails = new HashSet<string>(allProfiles.Where(p => p.EmailAddress != null).Select(p => p.EmailAddress!), StringComparer.OrdinalIgnoreCase);

        var staffNosInFile = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var emailsInFile = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        var previewRows = new List<ImportPreviewRow>();

        foreach (var row in rows)
        {
            var rowErrors = new List<string>();
            var rowNum = rows.IndexOf(row) + 2;

            if (string.IsNullOrWhiteSpace(row.StaffNo))
                rowErrors.Add("StaffNo is required.");
            else if (!staffNosInFile.Add(row.StaffNo))
                rowErrors.Add("Duplicate StaffNo in file.");
            else if (existingStaffNos.Contains(row.StaffNo))
                rowErrors.Add($"StaffNo '{row.StaffNo}' already exists.");

            if (!string.IsNullOrWhiteSpace(row.EmailAddress))
            {
                if (!emailsInFile.Add(row.EmailAddress))
                    rowErrors.Add("Duplicate Email in file.");
                else if (existingEmails.Contains(row.EmailAddress))
                    rowErrors.Add($"Email '{row.EmailAddress}' already exists.");
            }

            if (string.IsNullOrWhiteSpace(row.EmploymentStatus))
                rowErrors.Add("EmploymentStatus is required.");

            if (!string.IsNullOrWhiteSpace(row.DepartmentName) &&
                !departments.ContainsKey(row.DepartmentName))
                rowErrors.Add($"Department '{row.DepartmentName}' not found.");

            if (!string.IsNullOrWhiteSpace(row.PositionName) &&
                !positions.ContainsKey(row.PositionName))
                rowErrors.Add($"Position '{row.PositionName}' not found.");

            if (!string.IsNullOrWhiteSpace(row.TeamName) &&
                !teams.ContainsKey(row.TeamName))
                rowErrors.Add($"Team '{row.TeamName}' not found.");

            previewRows.Add(new ImportPreviewRow
            {
                RowNumber = rowNum,
                StaffNo = row.StaffNo ?? "",
                StaffName = row.StaffName ?? "",
                EmailAddress = row.EmailAddress,
                DepartmentName = row.DepartmentName,
                TeamName = row.TeamName,
                PositionName = row.PositionName,
                EmploymentStatus = row.EmploymentStatus,
                IsValid = rowErrors.Count == 0,
                Errors = rowErrors,
                Data = row
            });
        }

        var result = new ImportPreviewResult
        {
            TotalRows = rows.Count,
            ValidCount = previewRows.Count(r => r.IsValid),
            ErrorCount = previewRows.Count(r => !r.IsValid),
            Rows = previewRows
        };

        return SuccessResponse<ImportPreviewResult>.Ok(result,
            $"{result.ValidCount} valid, {result.ErrorCount} with errors.");
    }
}
