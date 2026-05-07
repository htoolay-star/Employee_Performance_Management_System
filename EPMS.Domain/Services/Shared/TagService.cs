using AutoMapper;
using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Shared;
using EPMS.Domain.Interface.Irepo.Shared;
using EPMS.Domain.Interface.IService.Shared;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.TagDTOs;
using EPMS.Shared.Enums;
using static EPMS.Shared.Constants.ServiceResponseMessages;

namespace EPMS.Domain.Services.Shared;

public class TagService : ITagService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TagService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<SuccessResponse<IEnumerable<TagDto>>> GetAllTagsAsync()
    {
        var tags = await _unitOfWork.Shared.Tags.GetAllAsync();
        var dtos = _mapper.Map<IEnumerable<TagDto>>(tags);
        return SuccessResponse<IEnumerable<TagDto>>.Ok(dtos, TagMsg.RetrievedAll);
    }

    public async Task<SuccessResponse<TagDto>> GetTagByIdAsync(int id)
    {
        var tag = await _unitOfWork.Shared.Tags.GetByIdAsync(id);

        if (tag == null)
            return SuccessResponse<TagDto>.Fail(TagMsg.NotFound(id), ErrorType.NotFound);

        var dto = _mapper.Map<TagDto>(tag);
        return SuccessResponse<TagDto>.Ok(dto, TagMsg.Retrieved);
    }

    public async Task<SuccessResponse<long>> CreateTagAsync(CreateTagDto dto)
    {
        // Check for duplicate
        if (await _unitOfWork.Shared.Tags.ExistsByNameAsync(dto.Name, dto.Module))
        {
            var moduleMsg = string.IsNullOrWhiteSpace(dto.Module) ? "" : $" in module '{dto.Module}'";
            return SuccessResponse<long>.Fail($"Tag with name '{dto.Name}' already exists{moduleMsg}.", ErrorType.Conflict);
        }

        var tag = new Tag(dto.Name, dto.Module);
        _unitOfWork.Shared.Tags.Add(tag);
        await _unitOfWork.CompleteAsync();
        return SuccessResponse<long>.Ok(tag.Id, TagMsg.Created);
    }

    public async Task<SuccessResponse> UpdateTagAsync(int id, UpdateTagDto dto)
    {
        var tag = await _unitOfWork.Shared.Tags.GetByIdAsync(id);

        if (tag == null)
            return SuccessResponse.Fail(TagMsg.NotFound(id), ErrorType.NotFound);

        if (await _unitOfWork.Shared.Tags.ExistsByNameAsync(dto.Name, dto.Module, id))
        {
            var moduleMsg = string.IsNullOrWhiteSpace(dto.Module) ? "" : $" in module '{dto.Module}'";
            return SuccessResponse.Fail($"Tag with name '{dto.Name}' already exists{moduleMsg}.", ErrorType.Conflict);
        }

        tag.Update(dto.Name, dto.Module);
        await _unitOfWork.CompleteAsync();
        return SuccessResponse.Ok(TagMsg.Updated);
    }

    public async Task<SuccessResponse> DeleteTagAsync(int id)
    {
        var tag = await _unitOfWork.Shared.Tags.GetByIdAsync(id);

        if (tag == null)
            return SuccessResponse.Fail(TagMsg.NotFound(id), ErrorType.NotFound);

        _unitOfWork.Shared.Tags.Delete(tag);
        await _unitOfWork.CompleteAsync();
        return SuccessResponse.Ok(TagMsg.Deleted);
    }
}
