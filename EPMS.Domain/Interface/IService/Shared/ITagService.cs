using EPMS.Shared.DTOs.SharedDTOs.TagDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Interface.IService.Shared
{
    public interface ITagService
    {
        Task<IEnumerable<TagDto>> GetAllTagsAsync();
        Task CreateTagAsync(CreateTagDto dto);
        Task DeleteTagAsync(int id);
    }
}
