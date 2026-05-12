using EPMS.Api.Controllers.Common;
using EPMS.Domain.Interface.IService.Info;
using EPMS.Shared.Constants;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.EmployeeInfoDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EPMS.Api.Controllers.Info;

[Route("api/employee-payroll-infos")]
[ApiController]
[Authorize(Roles = RoleConstants.Admin)]
public class EmployeePayrollInfosController : ApiControllerBase
{
    private readonly IEmployeePayrollInfoService _payrollInfoService;

    public EmployeePayrollInfosController(IEmployeePayrollInfoService payrollInfoService)
    {
        _payrollInfoService = payrollInfoService;
    }

    [HttpGet]
    public async Task<ActionResult<SuccessResponse<IEnumerable<EmployeePayrollInfoDto>>>> GetAll()
    {
        var result = await _payrollInfoService.GetAllAsync();
        return HandleResult(result);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<SuccessResponse<EmployeePayrollInfoDto>>> GetById(long id)
    {
        var result = await _payrollInfoService.GetByIdAsync(id);
        return HandleResult(result);
    }

    [HttpGet("by-employee/{employeePublicId:guid}")]
    public async Task<ActionResult<SuccessResponse<EmployeePayrollInfoDto>>> GetByEmployeeId(Guid employeePublicId)
    {
        var result = await _payrollInfoService.GetByEmployeeIdAsync(employeePublicId);
        return HandleResult(result);
    }

    [HttpPost]
    public async Task<ActionResult<SuccessResponse<long>>> Create(CreateEmployeePayrollInfoDto dto)
    {
        var result = await _payrollInfoService.CreateAsync(dto);
        return HandleResult(result);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<SuccessResponse>> Update(long id, UpdateEmployeePayrollInfoDto dto)
    {
        var result = await _payrollInfoService.UpdateAsync(id, dto);
        return HandleResult(result);
    }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult<SuccessResponse>> Delete(long id)
    {
        var result = await _payrollInfoService.DeleteAsync(id);
        return HandleResult(result);
    }
}
