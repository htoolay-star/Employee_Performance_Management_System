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
    [Authorize(Roles = RoleConstants.Admin)]
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

        [HttpGet("{id}")]
        public async Task<ActionResult<SuccessResponse<PermissionDto>>> GetById(int id)
        {
            var result = await _permissionService.GetPermissionByIdAsync(id);
            return HandleResult(result);
        }

        [HttpPost]
        public async Task<ActionResult<SuccessResponse>> Create(CreatePermissionDto dto)
        {
            var result = await _permissionService.CreatePermissionAsync(dto);
            return HandleResult(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SuccessResponse>> Update(int id, UpdatePermissionDto dto)
        {
            var result = await _permissionService.UpdatePermissionAsync(id, dto);
            return HandleResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<SuccessResponse>> Delete(int id)
        {
            var result = await _permissionService.DeletePermissionAsync(id);
            return HandleResult(result);
        }
    }
}
