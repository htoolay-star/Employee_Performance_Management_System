using EPMS.Api.Controllers.Common;
using EPMS.Domain.Interface.IService.Shared;
using EPMS.Shared.Constants;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.TagDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EPMS.Api.Controllers.Shared;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = RoleConstants.Admin)]
public class TagsController : ApiControllerBase
{
    private readonly ITagService _tagService;

    public TagsController(ITagService tagService)
    {
        _tagService = tagService;
    }

    [HttpGet]
    public async Task<ActionResult<SuccessResponse<IEnumerable<TagDto>>>> GetAll()
    {
        var result = await _tagService.GetAllTagsAsync();
        return HandleResult(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<SuccessResponse<TagDto>>> GetById(int id)
    {
        var result = await _tagService.GetTagByIdAsync(id);
        return HandleResult(result);
    }

    [HttpPost]
    public async Task<ActionResult<SuccessResponse<long>>> Create(CreateTagDto dto)
    {
        var result = await _tagService.CreateTagAsync(dto);
        return HandleResult(result);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<SuccessResponse>> Update(int id, UpdateTagDto dto)
    {
        var result = await _tagService.UpdateTagAsync(id, dto);
        return HandleResult(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<SuccessResponse>> Delete(int id)
    {
        var result = await _tagService.DeleteTagAsync(id);
        return HandleResult(result);
    }
}
