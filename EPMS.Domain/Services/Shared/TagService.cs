using AutoMapper;
using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Shared;
using EPMS.Domain.Interface.Irepo.Shared;
using EPMS.Domain.Interface.IService.Shared;
using EPMS.Shared.DTOs.SharedDTOs.TagDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Services.Shared
{
    public class TagService : ITagService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TagService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TagDto>> GetAllTagsAsync()
        {
            var tags = await _unitOfWork.Shared.Tags.GetAllAsync();
            return _mapper.Map<IEnumerable<TagDto>>(tags);
        }

        public async Task CreateTagAsync(CreateTagDto dto)
        {
            var tag = new Tag(dto.Name, dto.Module);
            _unitOfWork.Shared.Tags.Add(tag);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteTagAsync(int id)
        {
            var tag = await _unitOfWork.Shared.Tags.GetByIdAsync(id);
            if (tag != null)
            {
                _unitOfWork.Shared.Tags.Delete(tag);
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}
