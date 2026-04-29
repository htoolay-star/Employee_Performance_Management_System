using EPMS.Domain.Interface.IService.Shared;
using EPMS.Domain.Services.Shared;
using EPMS.Shared.DTOs.SharedDTOs.TagDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EPMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ITagService _tagService;

        public TagsController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TagDto>>> GetAll()
        {
            var tags = await _tagService.GetAllTagsAsync();
            return Ok(tags);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTagDto dto)
        {
            await _tagService.CreateTagAsync(dto);
            return Ok(new { message = "Successful updated Category" });
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _tagService.DeleteTagAsync(id);
            return Ok(new { message = "Successful Deleted Category" });
           
        }
    }
}
