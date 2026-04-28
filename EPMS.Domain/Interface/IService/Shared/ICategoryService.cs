using EPMS.Shared.DTOs.SharedDTOs.CategoryDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Interface.IService.Shared
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
        Task<CategoryDto?> GetCategoryByIdAsync(int id);
        Task CreateCategoryAsync(CreateCategoryDto dto);
        Task UpdateCategoryAsync(int id, UpdateCategoryDto dto);
        Task DeleteCategoryAsync(int id);
    }
}
