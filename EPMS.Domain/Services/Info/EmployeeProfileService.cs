using AutoMapper;
using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.EmployeeInfo;
using EPMS.Domain.Interface.IService.Info;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.EmployeeInfoDTOs;
using EPMS.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace EPMS.Domain.Services.Info;

public class EmployeeProfileService : IEmployeeProfileService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public EmployeeProfileService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<SuccessResponse<IEnumerable<EmployeeProfileDto>>> GetAllAsync()
    {
        var profiles = await _uow.Info.EmployeeProfiles.GetAllAsync();
        var dtos = _mapper.Map<IEnumerable<EmployeeProfileDto>>(profiles);
        return SuccessResponse<IEnumerable<EmployeeProfileDto>>.Ok(dtos, "Employee profiles retrieved successfully.");
    }

    public async Task<SuccessResponse<EmployeeProfileDto>> GetByIdAsync(long id)
    {
        var profile = await _uow.Info.EmployeeProfiles.GetByIdAsync(id);

        if (profile == null)
            return SuccessResponse<EmployeeProfileDto>.Fail($"Employee profile with ID '{id}' was not found.", ErrorType.NotFound);

        var dto = _mapper.Map<EmployeeProfileDto>(profile);
        return SuccessResponse<EmployeeProfileDto>.Ok(dto, "Employee profile retrieved successfully.");
    }

    public async Task<SuccessResponse<EmployeeProfileDetailDto>> GetFullProfileAsync(long id)
    {
        var profile = await _uow.Info.EmployeeProfiles.GetByIdAsync(id);

        if (profile == null)
            return SuccessResponse<EmployeeProfileDetailDto>.Fail($"Employee profile with ID '{id}' was not found.", ErrorType.NotFound);

        var dto = _mapper.Map<EmployeeProfileDetailDto>(profile);
        
        // Load related data
        var employment = await _uow.Info.EmployeeEmployments.GetByEmployeeIdAsync(id);
        dto = dto with { Employment = employment != null ? _mapper.Map<EmployeeEmploymentDto>(employment) : null };

        var contact = await _uow.Info.EmployeeContacts.GetByEmployeeIdAsync(id);
        dto = dto with { Contact = contact != null ? _mapper.Map<EmployeeContactDto>(contact) : null };

        var payroll = await _uow.Info.EmployeePayrollInfos.GetByEmployeeIdAsync(id);
        dto = dto with { PayrollInfo = payroll != null ? _mapper.Map<EmployeePayrollInfoDto>(payroll) : null };

        var family = await _uow.Info.EmployeeFamilyInfos.GetByEmployeeIdAsync(id);
        dto = dto with { FamilyInfo = family.FirstOrDefault() != null ? _mapper.Map<EmployeeFamilyInfoDto>(family.First()) : null };

        return SuccessResponse<EmployeeProfileDetailDto>.Ok(dto, "Full employee profile retrieved successfully.");
    }

    public async Task<SuccessResponse<long>> CreateAsync(CreateEmployeeProfileDto dto)
    {
        // Check for duplicate StaffNo
        var existing = await _uow.Info.EmployeeProfiles.GetByStaffNoAsync(dto.StaffNo);
        if (existing != null)
            return SuccessResponse<long>.Fail($"Employee with staff number '{dto.StaffNo}' already exists.", ErrorType.Conflict);

        // Check for duplicate UserId if provided
        if (dto.UserId.HasValue)
        {
            var existingUser = await _uow.Info.EmployeeProfiles.GetByUserIdAsync(dto.UserId.Value);
            if (existingUser != null)
                return SuccessResponse<long>.Fail($"Employee profile for user ID '{dto.UserId.Value}' already exists.", ErrorType.Conflict);
        }

        var profile = new EmployeeProfile(dto.UserId, dto.StaffNo, dto.FirstName, dto.LastName);
        
        // Set additional properties
        if (!string.IsNullOrEmpty(dto.OtherName)) profile.GetType().GetProperty("OtherName")?.SetValue(profile, dto.OtherName);
        if (!string.IsNullOrEmpty(dto.NRCNo)) profile.GetType().GetProperty("NRCNo")?.SetValue(profile, dto.NRCNo);
        if (!string.IsNullOrEmpty(dto.Gender)) profile.UpdateDemographics(dto.Gender, dto.DateOfBirth, dto.Nationality);
        
        _uow.Info.EmployeeProfiles.Add(profile);
        await _uow.CompleteAsync();
        
        return SuccessResponse<long>.Ok(profile.Id, "Employee profile created successfully.");
    }

    public async Task<SuccessResponse> UpdateAsync(long id, UpdateEmployeeProfileDto dto)
    {
        var profile = await _uow.Info.EmployeeProfiles.GetByIdAsync(id);

        if (profile == null)
            return SuccessResponse.Fail($"Employee profile with ID '{id}' was not found.", ErrorType.NotFound);

        profile.UpdateDemographics(dto.Gender, dto.DateOfBirth, dto.Nationality);
        
        if (!string.IsNullOrEmpty(dto.WorkPermitNo))
            profile.UpdateWorkPermit(dto.WorkPermitNo, dto.WorkPermitValidDate, dto.WorkPermitExpireDate);
        
        if (!string.IsNullOrEmpty(dto.ProfilePictureUrl))
            profile.UpdateProfilePicture(dto.ProfilePictureUrl, dto.ProfileThumbnailUrl);
        
        if (!string.IsNullOrEmpty(dto.AdditionalData))
            profile.UpdateAdditionalData(dto.AdditionalData);

        await _uow.CompleteAsync();
        return SuccessResponse.Ok("Employee profile updated successfully.");
    }

    public async Task<SuccessResponse> DeleteAsync(long id)
    {
        var profile = await _uow.Info.EmployeeProfiles.GetByIdAsync(id);

        if (profile == null)
            return SuccessResponse.Fail($"Employee profile with ID '{id}' was not found.", ErrorType.NotFound);

        _uow.Info.EmployeeProfiles.Delete(profile);
        await _uow.CompleteAsync();
        
        return SuccessResponse.Ok("Employee profile deleted successfully.");
    }

    public async Task<SuccessResponse<EmployeeProfileDto>> GetByStaffNoAsync(string staffNo)
    {
        var profile = await _uow.Info.EmployeeProfiles.GetByStaffNoAsync(staffNo);

        if (profile == null)
            return SuccessResponse<EmployeeProfileDto>.Fail($"Employee with staff number '{staffNo}' was not found.", ErrorType.NotFound);

        var dto = _mapper.Map<EmployeeProfileDto>(profile);
        return SuccessResponse<EmployeeProfileDto>.Ok(dto, "Employee profile retrieved successfully.");
    }

    public async Task<SuccessResponse<EmployeeProfileDto>> GetByUserIdAsync(long userId)
    {
        var profile = await _uow.Info.EmployeeProfiles.GetByUserIdAsync(userId);

        if (profile == null)
            return SuccessResponse<EmployeeProfileDto>.Fail($"Employee profile for user ID '{userId}' was not found.", ErrorType.NotFound);

        var dto = _mapper.Map<EmployeeProfileDto>(profile);
        return SuccessResponse<EmployeeProfileDto>.Ok(dto, "Employee profile retrieved successfully.");
    }
}
