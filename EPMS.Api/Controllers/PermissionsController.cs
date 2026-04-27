using EPMS.Domain.Interface.IService;
using EPMS.Shared.PermissionDTOs;
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
            if (permission == null) return NotFound("Permission ရှာမတွေ့ပါ");

            return Ok(permission);
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreatePermissionDto dto)
        {
            try
            {
                await _permissionService.CreatePermissionAsync(dto);
                return Ok(new { message = "Permission အောင်မြင်စွာ သိမ်းဆည်းပြီးပါပြီ" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdatePermissionDto dto)
        {
            try
            {
                await _permissionService.UpdatePermissionAsync(id, dto);
                return Ok(new { message = "ပြင်ဆင်မှု အောင်မြင်ပါသည်" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _permissionService.DeletePermissionAsync(id);
                return Ok(new { message = "ဖျက်သိမ်းပြီးပါပြီ" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
