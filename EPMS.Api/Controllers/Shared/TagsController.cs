using EPMS.Domain.Interface.IService.Shared;
using EPMS.Shared.DTOs.TagDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EPMS.Api.Controllers.Shared
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
            return Ok(new { message = "Successful create new Tag" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _tagService.DeleteTagAsync(id);
            return Ok(new { message = "Successful Delelted Tag" });
        }
    }
}
