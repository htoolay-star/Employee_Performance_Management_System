using EPMS.Domain.Contracts;
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
    private readonly IPositionPermissionChecker _permissionChecker;

    public EmployeeProfileService(
        IUnitOfWork uow,
        ICurrentEmployeeContextService currentEmployee,
        IPositionPermissionChecker permissionChecker)
    {
        _uow = uow;
        _currentEmployee = currentEmployee;
        _permissionChecker = permissionChecker;
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

    public async Task<SuccessResponse<EmployeeProfileDetailDto>> GetFullProfileAsync(long id)
    {
        var positionId = await _currentEmployee.GetPositionIdAsync();
        if (!positionId.HasValue)
            return SuccessResponse<EmployeeProfileDetailDto>.Fail("User position is required.", ErrorType.Forbidden);

        var canView = await _permissionChecker.HasPermissionAsync(positionId.Value, PermissionCodes.InfoEmployeeFullProfileView);
        if (!canView)
            return SuccessResponse<EmployeeProfileDetailDto>.Fail("Permission denied.", ErrorType.Forbidden);

        var profile = await _uow.Info.EmployeeProfiles.GetByIdAsync(id);

        if (profile == null)
            return SuccessResponse<EmployeeProfileDetailDto>.Fail(EmployeeProfileMsg.NotFound(id), ErrorType.NotFound);

        var dto = profile.Adapt<EmployeeProfileDetailDto>();

        var employment = await _uow.Info.EmployeeEmployments.GetByEmployeeIdAsync(id);
        dto = dto with { Employment = employment?.Adapt<EmployeeEmploymentDto>() };

        var contact = await _uow.Info.EmployeeContacts.GetByEmployeeIdAsync(id);
        dto = dto with { Contact = contact?.Adapt<EmployeeContactDto>() };

        var payroll = await _uow.Info.EmployeePayrollInfos.GetByEmployeeIdAsync(id);
        dto = dto with { PayrollInfo = payroll?.Adapt<EmployeePayrollInfoDto>() };

        var family = await _uow.Info.EmployeeFamilyInfos.GetByEmployeeIdAsync(id);
        dto = dto with { FamilyInfo = family.FirstOrDefault()?.Adapt<EmployeeFamilyInfoDto>() };

        return SuccessResponse<EmployeeProfileDetailDto>.Ok(dto, EmployeeProfileMsg.Retrieved);
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

        var profile = new EmployeeProfile(dto.UserId, dto.StaffNo, dto.FirstName, dto.LastName);
        
        // Set additional properties using entity methods
        if (!string.IsNullOrEmpty(dto.OtherName)) profile.UpdateOtherName(dto.OtherName);
        if (!string.IsNullOrEmpty(dto.NRCNo)) profile.UpdateNRCNo(dto.NRCNo);
        if (!string.IsNullOrEmpty(dto.Gender)) profile.UpdateDemographics(dto.Gender, dto.DateOfBirth, dto.Nationality);
        
        _uow.Info.EmployeeProfiles.Add(profile);
        await _uow.CompleteAsync();
        
        return SuccessResponse<long>.Ok(profile.Id, EmployeeProfileMsg.Created);
    }

    public async Task<SuccessResponse> UpdateAsync(long id, UpdateEmployeeProfileDto dto)
    {
        var profile = await _uow.Info.EmployeeProfiles.GetByIdAsync(id);

        if (profile == null)
            return SuccessResponse.Fail(EmployeeProfileMsg.NotFound(id), ErrorType.NotFound);

        profile.UpdateDemographics(dto.Gender, dto.DateOfBirth, dto.Nationality);
        
        if (!string.IsNullOrEmpty(dto.WorkPermitNo))
            profile.UpdateWorkPermit(dto.WorkPermitNo, dto.WorkPermitValidDate, dto.WorkPermitExpireDate);
        
        if (!string.IsNullOrEmpty(dto.ProfilePictureUrl))
            profile.UpdateProfilePicture(dto.ProfilePictureUrl, dto.ProfileThumbnailUrl);
        
        if (!string.IsNullOrEmpty(dto.AdditionalData))
            profile.UpdateAdditionalData(dto.AdditionalData);

        await _uow.CompleteAsync();
        return SuccessResponse.Ok(EmployeeProfileMsg.Updated);
    }

    public async Task<SuccessResponse> DeleteAsync(long id)
    {
        var profile = await _uow.Info.EmployeeProfiles.GetByIdAsync(id);

        if (profile == null)
            return SuccessResponse.Fail(EmployeeProfileMsg.NotFound(id), ErrorType.NotFound);

        _uow.Info.EmployeeProfiles.Delete(profile);
        await _uow.CompleteAsync();
        
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
}
