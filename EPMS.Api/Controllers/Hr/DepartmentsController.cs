using EPMS.Api.Authorization;
using EPMS.Api.Controllers.Common;
using EPMS.Domain.Interface.IService.Hr;
using EPMS.Shared.Constants;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.DepartmentDTOs;
using EPMS.Shared.DTOs.TeamDTOs;
using Microsoft.AspNetCore.Mvc;

namespace EPMS.Api.Controllers.Hr;

[ApiController]
[Route("api/[controller]")]
public class DepartmentsController : ApiControllerBase
{
    private readonly IDepartmentService _service;

    public DepartmentsController(IDepartmentService service)
    {
        _service = service;
    }

    [HasPermission("DEPARTMENTS.VIEW")]
    [HttpGet("lookup")]
    public async Task<ActionResult<SuccessResponse<IEnumerable<LookUpDto>>>> GetLookup()
    {
        var result = await _service.GetLookupAsync();
        return HandleResult(result);
    }

    [HasPermission("DEPARTMENTS.VIEW")]
    [HttpGet("with-teams")]
    public async Task<ActionResult<SuccessResponse<IEnumerable<DepartmentDto>>>> GetWithTeams([FromQuery] long? teamId = null)
    {
        var result = await _service.GetDepartmentWithTeamsAsync(teamId ?? 0);
        return HandleResult(result);
    }

    [HasPermission("DEPARTMENTS.VIEW")]
    [HttpGet]
    public async Task<ActionResult<SuccessResponse<IEnumerable<DepartmentDto>>>> GetAll()
    {
        var result = await _service.GetAllAsync();
        return HandleResult(result);
    }

    [HasPermission("DEPARTMENTS.VIEW")]
    [HttpGet("{id:long}")]
    public async Task<ActionResult<SuccessResponse<DepartmentDto>>> GetById(long id)
    {
        var result = await _service.GetByIdAsync(id);
        return HandleResult(result);
    }

    [HasPermission("DEPARTMENTS.CREATE")]
    [HttpPost]
    public async Task<ActionResult<SuccessResponse<long>>> Create(CreateDepartmentDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return HandleResult(result);
    }

    [HasPermission("DEPARTMENTS.EDIT")]
    [HttpPut("{id:long}")]
    public async Task<ActionResult<SuccessResponse>> Update(long id, UpdateDepartmentDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        return HandleResult(result);
    }

    [HasPermission("DEPARTMENTS.DELETE")]
    [HttpDelete("{id:long}")]
    public async Task<ActionResult<SuccessResponse>> Delete(long id)
    {
        var result = await _service.DeleteAsync(id);
        return HandleResult(result);
    }
        [HasPermission("DEPARTMENTS.DELETE")]
        [HttpPost("{id:long}/restore")]
        public async Task<ActionResult<SuccessResponse>> Restore(long id)
        {
            var result = await _service.RestoreAsync(id);
            return HandleResult(result);
        }
}