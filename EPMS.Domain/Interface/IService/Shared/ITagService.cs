using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.TagDTOs;

namespace EPMS.Domain.Interface.IService.Shared;

public interface ITagService
{
    Task<SuccessResponse<IEnumerable<LookUpDto>>> GetLookupAsync();   
    Task<SuccessResponse<IEnumerable<TagDto>>> GetAllTagsAsync();
    Task<SuccessResponse<TagDto>> GetTagByIdAsync(long id);
    Task<SuccessResponse<long>> CreateTagAsync(CreateTagDto dto);
    Task<SuccessResponse> UpdateTagAsync(long id, UpdateTagDto dto);
    Task<SuccessResponse> DeleteTagAsync(long id);
}
