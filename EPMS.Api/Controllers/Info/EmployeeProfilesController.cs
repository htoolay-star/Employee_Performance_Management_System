using EPMS.Api.Controllers.Common;
using EPMS.Domain.Interface.IService.Info;
using EPMS.Shared.Constants;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.EmployeeInfoDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EPMS.Api.Controllers.Info;

[Route("api/employee-profiles")]
[ApiController]
public class EmployeeProfilesController : ApiControllerBase
{
    private readonly IEmployeeProfileService _profileService;

    public EmployeeProfilesController(IEmployeeProfileService profileService)
    {
        _profileService = profileService;
    }

    [HttpGet]
    public async Task<ActionResult<SuccessResponse<IEnumerable<EmployeeProfileDto>>>> GetAll()
    {
        var result = await _profileService.GetAllAsync();
        return HandleResult(result);
    }

    [HttpGet("lookup")]
    public async Task<ActionResult<SuccessResponse<IEnumerable<EmployeeLookupDto>>>> GetLookup()
    {
        var result = await _profileService.GetLookupAsync();
        return HandleResult(result);
    }

    [HttpGet("paged")]
    public async Task<ActionResult<SuccessResponse<PaginatedResponse<EmployeeProfileGridItemDto>>>> GetPaged([FromQuery] EPMS.Shared.Features.EmployeeProfiles.EmployeeProfileQueryParameters parameters)
    {
        var result = await _profileService.GetPagedAsync(parameters);
        return HandleResult(result);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<SuccessResponse<EmployeeProfileDto>>> GetById(long id)
    {
        var result = await _profileService.GetByIdAsync(id);
        return HandleResult(result);
    }

    [HttpGet("by-staffno/{staffNo}")]
    public async Task<ActionResult<SuccessResponse<EmployeeProfileDto>>> GetByStaffNo(string staffNo)
    {
        var result = await _profileService.GetByStaffNoAsync(staffNo);
        return HandleResult(result);
    }

    [HttpGet("by-user/{userId:long}")]
    public async Task<ActionResult<SuccessResponse<EmployeeProfileDto>>> GetByUserId(long userId)
    {
        var result = await _profileService.GetByUserIdAsync(userId);
        return HandleResult(result);
    }

    [HttpPost]
    public async Task<ActionResult<SuccessResponse<long>>> Create(CreateEmployeeProfileDto dto)
    {
        var result = await _profileService.CreateAsync(dto);
        return HandleResult(result);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<SuccessResponse>> Update(long id, UpdateEmployeeProfileDto dto)
    {
        var result = await _profileService.UpdateAsync(id, dto);
        return HandleResult(result);
    }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult<SuccessResponse>> Delete(long id)
    {
        var result = await _profileService.DeleteAsync(id);
        return HandleResult(result);
    }
}
