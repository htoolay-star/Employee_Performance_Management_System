using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.TagDTOs;

namespace EPMS.Domain.Interface.IService.Shared;

public interface ITagService
{
    Task<SuccessResponse<IEnumerable<TagDto>>> GetAllTagsAsync();
    Task<SuccessResponse<TagDto>> GetTagByIdAsync(int id);
    Task<SuccessResponse<long>> CreateTagAsync(CreateTagDto dto);
    Task<SuccessResponse> UpdateTagAsync(int id, UpdateTagDto dto);
    Task<SuccessResponse> DeleteTagAsync(int id);
}
