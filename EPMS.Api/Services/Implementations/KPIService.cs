using AutoMapper;
using EPMS.Api.Services.Interfaces;
using EPMS.Domain.Entities.Performance;
using EPMS.Shared.DTOs.KPI;

namespace EPMS.Api.Services.Implementations
{
    public class KPIService : IKPIService
    {
        private readonly IKPIMasterRepository _repo;
        private readonly IMapper _mapper;

        public KPIService(
            IKPIMasterRepository repo,
            IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<KPIMasterDto>> GetAllAsync()
        {
            var entities = await _repo.GetAllAsync();

            return _mapper.Map<List<KPIMasterDto>>(entities);
        }

        public async Task CreateAsync(CreateKPIMasterDto dto)
        {
            var entity = new KPIMaster(
                dto.CategoryId,
                dto.Code,
                dto.Name,
                dto.Description);

            await _repo.AddAsync(entity);

            await _repo.SaveChangesAsync();
        }
    }
}
