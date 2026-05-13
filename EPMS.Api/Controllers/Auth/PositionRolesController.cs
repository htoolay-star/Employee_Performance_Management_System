using EPMS.Api.Controllers.Common;
using EPMS.Domain.Interface.IService.Auth;
using EPMS.Shared.DTOs.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EPMS.Api.Controllers.Auth;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PositionRolesController : ApiControllerBase
{
    private readonly IPositionRoleService _service;

    public PositionRolesController(IPositionRoleService service)
    {
        _service = service;
    }
}