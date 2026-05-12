using EPMS.Api.Controllers.Common;
using EPMS.Domain.Interface.IService.Info;
using EPMS.Shared.Constants;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.EmployeeInfoDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EPMS.Api.Controllers.Info;

[Route("api/employee-contacts")]
[ApiController]
[Authorize(Roles = RoleConstants.Admin)]
public class EmployeeContactsController : ApiControllerBase
{
    private readonly IEmployeeContactService _contactService;

    public EmployeeContactsController(IEmployeeContactService contactService)
    {
        _contactService = contactService;
    }

    [HttpGet]
    public async Task<ActionResult<SuccessResponse<IEnumerable<EmployeeContactDto>>>> GetAll()
    {
        var result = await _contactService.GetAllAsync();
        return HandleResult(result);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<SuccessResponse<EmployeeContactDto>>> GetById(long id)
    {
        var result = await _contactService.GetByIdAsync(id);
        return HandleResult(result);
    }

    [HttpGet("by-employee/{employeePublicId:guid}")]
    public async Task<ActionResult<SuccessResponse<EmployeeContactDto>>> GetByEmployeeId(Guid employeePublicId)
    {
        var result = await _contactService.GetByEmployeeIdAsync(employeePublicId);
        return HandleResult(result);
    }

    [HttpPost]
    public async Task<ActionResult<SuccessResponse<long>>> Create(CreateEmployeeContactDto dto)
    {
        var result = await _contactService.CreateAsync(dto);
        return HandleResult(result);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<SuccessResponse>> Update(long id, UpdateEmployeeContactDto dto)
    {
        var result = await _contactService.UpdateAsync(id, dto);
        return HandleResult(result);
    }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult<SuccessResponse>> Delete(long id)
    {
        var result = await _contactService.DeleteAsync(id);
        return HandleResult(result);
    }
}
