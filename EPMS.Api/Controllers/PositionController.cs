using EPMS.Application.Services.PositionService;
using EPMS.Shared.PositionDTOs;
using Microsoft.AspNetCore.Mvc;

namespace HRApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PositionController : ControllerBase
    {
        private readonly IPositionService _service;

        public PositionController(IPositionService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePositionDto dto)
        {
            var id = await _service.CreateAsync(dto);
            return Ok(new { Message = "Created Successfully", Id = id });
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdatePositionDto dto)
        {
            var ok = await _service.UpdateAsync(dto);
            return ok ? Ok("Updated Successfully") : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteAsync(id);
            return ok ? Ok("Deleted Successfully") : NotFound();
        }
    }
}