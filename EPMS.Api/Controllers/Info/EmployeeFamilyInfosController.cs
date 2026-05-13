using EPMS.Api.Controllers.Common;
using EPMS.Domain.Interface.IService.Info;
using EPMS.Shared.Constants;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.EmployeeInfoDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EPMS.Api.Controllers.Info;

[Route("api/employee-family-infos")]
[ApiController]
public class EmployeeFamilyInfosController : ApiControllerBase
{
    private readonly IEmployeeFamilyInfoService _familyInfoService;

    public EmployeeFamilyInfosController(IEmployeeFamilyInfoService familyInfoService)
    {
        _familyInfoService = familyInfoService;
    }

    [HttpGet]
    public async Task<ActionResult<SuccessResponse<IEnumerable<EmployeeFamilyInfoDto>>>> GetAll()
    {
        var result = await _familyInfoService.GetAllAsync();
        return HandleResult(result);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<SuccessResponse<EmployeeFamilyInfoDto>>> GetById(long id)
    {
        var result = await _familyInfoService.GetByIdAsync(id);
        return HandleResult(result);
    }

    [HttpGet("by-employee/{employeePublicId:guid}")]
    public async Task<ActionResult<SuccessResponse<EmployeeFamilyInfoDto>>> GetByEmployeeId(Guid employeePublicId)
    {
        var result = await _familyInfoService.GetByEmployeeIdAsync(employeePublicId);
        return HandleResult(result);
    }

    [HttpPost]
    public async Task<ActionResult<SuccessResponse<long>>> Create(CreateEmployeeFamilyInfoDto dto)
    {
        var result = await _familyInfoService.CreateAsync(dto);
        return HandleResult(result);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<SuccessResponse>> Update(long id, UpdateEmployeeFamilyInfoDto dto)
    {
        var result = await _familyInfoService.UpdateAsync(id, dto);
        return HandleResult(result);
    }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult<SuccessResponse>> Delete(long id)
    {
        var result = await _familyInfoService.DeleteAsync(id);
        return HandleResult(result);
    }
}
