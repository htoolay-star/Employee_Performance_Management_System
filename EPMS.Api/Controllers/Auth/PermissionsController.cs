using EPMS.Api.Controllers.Common;
using EPMS.Domain.Interface.IService.Auth;
using EPMS.Shared.Constants;
using EPMS.Shared.DTOs.AuthDTOs.PermissionDTOS;
using EPMS.Shared.DTOs.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EPMS.Api.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.SystemAdmin}")]
    public class PermissionsController : ApiControllerBase
    {
        private readonly IPermissionService _permissionService;

        public PermissionsController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        [HttpGet]
        public async Task<ActionResult<SuccessResponse<IEnumerable<PermissionDto>>>> GetAll()
        {
            var result = await _permissionService.GetAllPermissionsAsync();
            return HandleResult(result);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<SuccessResponse<PermissionDto>>> GetById(long id)
        {
            var result = await _permissionService.GetPermissionByIdAsync(id);
            return HandleResult(result);
        }
    }
}
