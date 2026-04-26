using EPMS.Domain.Interface.Irepo.IPositionRepository;
using EPMS.Shared.PositionDTOs;
using Microsoft.AspNetCore.Mvc;

namespace HRApi.Controllers
{
   
    [ApiController]
    [Route("api/[controller]")]
    public class PositionController : ControllerBase
    {
        private readonly IPositionRepository _repo;
        public PositionController(IPositionRepository repo) => _repo = repo;

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _repo.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _repo.GetByIdAsync(id);
            return data == null ? NotFound() : Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePositionDto dto)
        {
            var (ok, msg, newId) = await _repo.CreateAsync(dto);
            return ok
                ? Ok(new { Message = msg, Position_ID = newId })
                : BadRequest(new { Message = msg });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdatePositionDto dto)
        {
            var (ok, msg) = await _repo.UpdateAsync(id, dto);
            return ok ? Ok(new { Message = msg }) : BadRequest(new { Message = msg });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var (ok, msg) = await _repo.DeleteAsync(id);
            return ok ? Ok(new { Message = msg }) : BadRequest(new { Message = msg });
        }
    }
}
