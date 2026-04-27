using EPMS.Domain.Interface.IService;
using EPMS.Shared.DTOs.PermissionDTOS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EPMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        private readonly IPermissionService _permissionService;

        public PermissionsController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PermissionDto>>> GetAll()
        {
            var permissions = await _permissionService.GetAllPermissionsAsync();
            return Ok(permissions);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<PermissionDto>> GetById(int id)
        {
            var permission = await _permissionService.GetPermissionByIdAsync(id);

            return Ok(permission);
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreatePermissionDto dto)
        {
            await _permissionService.CreatePermissionAsync(dto);
            return Ok(new { message = "Permission Created successfully." });
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdatePermissionDto dto)
        {
            await _permissionService.UpdatePermissionAsync(id, dto);
            return Ok(new { message = "Permission Updated Successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _permissionService.DeletePermissionAsync(id);
            return Ok(new { message = "Permission Deleted Successfully" });
        }
    }
}
