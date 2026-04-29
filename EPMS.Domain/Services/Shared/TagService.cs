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
        private readonly ITagRepository _tagRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TagService(ITagRepository tagRepo, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _tagRepo = tagRepo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TagDto>> GetAllTagsAsync()
        {
            var tags = await _tagRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<TagDto>>(tags);


        }

        public async Task CreateTagAsync(CreateTagDto dto)
        {
            var normalizedName = dto.Name.Trim().ToLowerInvariant();
            var isExist = await _tagRepo.AnyAsync(x => x.Name == normalizedName && x.Module == dto.Module);

            if (isExist)
            {
                throw new Exception("Tag name already exists in this module.");
            }

            var tag = new Tag(dto.Name, dto.Module);
            _tagRepo.Add(tag);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteTagAsync(int id)
        {
            var tag = await _tagRepo.GetByIdAsync(id);
            if (tag != null)
            {
                _tagRepo.Delete(tag);
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}
