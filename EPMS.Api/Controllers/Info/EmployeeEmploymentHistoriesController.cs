using EPMS.Api.Controllers.Common;
using EPMS.Domain.Interface.IService.Info;
using EPMS.Shared.Constants;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.EmployeeInfoDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EPMS.Api.Controllers.Info;

[Route("api/employee-employment-histories")]
[ApiController]
public class EmployeeEmploymentHistoriesController : ApiControllerBase
{
    private readonly IEmployeeEmploymentHistoryService _historyService;

    public EmployeeEmploymentHistoriesController(IEmployeeEmploymentHistoryService historyService)
    {
        _historyService = historyService;
    }

    [HttpGet]
    public async Task<ActionResult<SuccessResponse<IEnumerable<EmployeeEmploymentHistoryDto>>>> GetAll()
    {
        var result = await _historyService.GetAllAsync();
        return HandleResult(result);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<SuccessResponse<EmployeeEmploymentHistoryDto>>> GetById(long id)
    {
        var result = await _historyService.GetByIdAsync(id);
        return HandleResult(result);
    }

    [HttpGet("by-employee/{employeeId:long}")]
    public async Task<ActionResult<SuccessResponse<IEnumerable<EmployeeEmploymentHistoryDto>>>> GetByEmployeeId(long employeeId)
    {
        var result = await _historyService.GetByEmployeeIdAsync(employeeId);
        return HandleResult(result);
    }
}
