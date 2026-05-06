using EPMS.Api.Controllers.Common;
using EPMS.Domain.Interface.IService.Shared;
using EPMS.Shared.Constants;
using EPMS.Shared.DTOs.CategoryDTOs;
using EPMS.Shared.DTOs.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EPMS.Api.Controllers.Shared;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = RoleConstants.Admin)]
public class CategoriesController : ApiControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<ActionResult<SuccessResponse<IEnumerable<CategoryDto>>>> GetAll()
    {
        var result = await _categoryService.GetAllCategoriesAsync();
        return HandleResult(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<SuccessResponse<CategoryDto>>> GetById(int id)
    {
        var result = await _categoryService.GetCategoryByIdAsync(id);
        return HandleResult(result);
    }

    [HttpPost]
    public async Task<ActionResult<SuccessResponse<long>>> Create(CreateCategoryDto dto)
    {
        var result = await _categoryService.CreateCategoryAsync(dto);
        return HandleResult(result);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<SuccessResponse>> Update(int id, UpdateCategoryDto dto)
    {
        var result = await _categoryService.UpdateCategoryAsync(id, dto);
        return HandleResult(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<SuccessResponse>> Delete(int id)
    {
        var result = await _categoryService.DeleteCategoryAsync(id);
        return HandleResult(result);
    }
}
