using AutoMapper;
using EPMS.Api.Services.Interfaces;
using EPMS.Domain.Entities.PerformanceImprovementPlan;
using EPMS.Domain.Repositories.Interfaces;
using EPMS.Shared.DTOs.PerformanceImprovementPlan;

namespace EPMS.Api.Services.Implementations
{
    public class PIPService : IPIPService
    {
        private readonly IPIPRepository _repo;
        private readonly IMapper _mapper;

        public PIPService(IPIPRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<PIPDto> CreateAsync(CreatePIPDto dto)
        {
            var entity = new PerformanceImprovementPlan
            {
                Id = Guid.NewGuid(),
                EmployeeId = dto.EmployeeId,
                Objectives = dto.Objectives,
                StartDate = DateTime.UtcNow,
                EndDate = (DateTime)dto.EndDate,
                Status = "Active"
            };

            await _repo.AddAsync(entity);

            return _mapper.Map<PIPDto>(entity);
        }

        public async Task AddProgressAsync(AddPIPProgressDto dto)
        {
            var pip = await _repo.GetByIdAsync(dto.PIPId);

            if (pip == null)
                throw new Exception("PIP not found");

            pip.ProgressUpdates.Add(new PIPProgress
            {
                Id = Guid.NewGuid(),
                PIPId = pip.Id,
                Feedback = dto.Feedback,
                ProgressStatus = dto.ProgressStatus,
                ReviewDate = DateTime.UtcNow
            });

            await _repo.SaveChangesAsync();
        }

        public async Task<List<PIPDto>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();
            return _mapper.Map<List<PIPDto>>(list);
        }
    }
}
