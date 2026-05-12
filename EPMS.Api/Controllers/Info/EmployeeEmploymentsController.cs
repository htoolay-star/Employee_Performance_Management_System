using EPMS.Api.Controllers.Common;
using EPMS.Domain.Interface.IService.Info;
using EPMS.Shared.Constants;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.EmployeeInfoDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EPMS.Api.Controllers.Info;

[Route("api/employee-employments")]
[ApiController]
[Authorize(Roles = RoleConstants.Admin)]
public class EmployeeEmploymentsController : ApiControllerBase
{
    private readonly IEmployeeEmploymentService _employmentService;

    public EmployeeEmploymentsController(IEmployeeEmploymentService employmentService)
    {
        _employmentService = employmentService;
    }

    [HttpGet]
    public async Task<ActionResult<SuccessResponse<IEnumerable<EmployeeEmploymentDto>>>> GetAll()
    {
        var result = await _employmentService.GetAllAsync();
        return HandleResult(result);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<SuccessResponse<EmployeeEmploymentDto>>> GetById(long id)
    {
        var result = await _employmentService.GetByIdAsync(id);
        return HandleResult(result);
    }

    [HttpGet("by-employee/{employeePublicId:guid}")]
    public async Task<ActionResult<SuccessResponse<EmployeeEmploymentDto>>> GetByEmployeeId(Guid employeePublicId)
    {
        var result = await _employmentService.GetByEmployeeIdAsync(employeePublicId);
        return HandleResult(result);
    }

    [HttpPost]
    public async Task<ActionResult<SuccessResponse<long>>> Create(CreateEmployeeEmploymentDto dto)
    {
        var result = await _employmentService.CreateAsync(dto);
        return HandleResult(result);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<SuccessResponse>> Update(long id, UpdateEmployeeEmploymentDto dto)
    {
        var result = await _employmentService.UpdateAsync(id, dto);
        return HandleResult(result);
    }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult<SuccessResponse>> Delete(long id)
    {
        var result = await _employmentService.DeleteAsync(id);
        return HandleResult(result);
    }
}
