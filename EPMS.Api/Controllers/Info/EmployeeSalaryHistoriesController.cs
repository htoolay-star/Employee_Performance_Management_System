using EPMS.Api.Controllers.Common;
using EPMS.Domain.Interface.IService.Info;
using EPMS.Shared.Constants;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.EmployeeInfoDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EPMS.Api.Controllers.Info;

[Route("api/employee-salary-histories")]
[ApiController]
[Authorize(Roles = RoleConstants.Admin)]
public class EmployeeSalaryHistoriesController : ApiControllerBase
{
    private readonly IEmployeeSalaryHistoryService _historyService;

    public EmployeeSalaryHistoriesController(IEmployeeSalaryHistoryService historyService)
    {
        _historyService = historyService;
    }

    [HttpGet]
    public async Task<ActionResult<SuccessResponse<IEnumerable<EmployeeSalaryHistoryDto>>>> GetAll()
    {
        var result = await _historyService.GetAllAsync();
        return HandleResult(result);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<SuccessResponse<EmployeeSalaryHistoryDto>>> GetById(long id)
    {
        var result = await _historyService.GetByIdAsync(id);
        return HandleResult(result);
    }

    [HttpGet("by-employee/{employeeId:long}")]
    public async Task<ActionResult<SuccessResponse<IEnumerable<EmployeeSalaryHistoryDto>>>> GetByEmployeeId(long employeeId)
    {
        var result = await _historyService.GetByEmployeeIdAsync(employeeId);
        return HandleResult(result);
    }
}
