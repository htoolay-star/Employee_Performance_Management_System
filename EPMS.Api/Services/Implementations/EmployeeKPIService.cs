using AutoMapper;
using EPMS.Api.Services.Interfaces;
using EPMS.Domain.Entities.Performance;
using EPMS.Shared.DTOs.KPI;

namespace EPMS.Api.Services.Implementations
{
    public class EmployeeKPIService : IEmployeeKPIService
    {
        private readonly IEmployeeKPIResultRepository _repo;
        private readonly IPositionKPIRepository _positionRepo;
        private readonly IMapper _mapper;

        public EmployeeKPIService(
            IEmployeeKPIResultRepository repo,
            IPositionKPIRepository positionRepo,
            IMapper mapper)
        {
            _repo = repo;
            _positionRepo = positionRepo;
            _mapper = mapper;
        }

        public async Task SubmitAsync(SubmitEmployeeKPIDto dto)
        {
            var positionKpi =
                await _positionRepo.GetByIdAsync(dto.PositionKPIId);

            if (positionKpi is null)
                throw new Exception("Position KPI not found.");

            var entity = new EmployeeKPIResult(
                dto.EmployeeId,
                dto.PositionKPIId,
                dto.PerformanceCycleId,
                dto.TargetValue,
                dto.ActualValue,
                dto.IsNegativeKPI);

            await _repo.AddAsync(entity);

            await _repo.SaveChangesAsync();
        }

        public async Task<List<EmployeeKPIResultDto>>
            GetEmployeeResultsAsync(long employeeId, long cycleId)
        {
            var results =
                await _repo.GetByEmployeeAsync(employeeId, cycleId);

            return _mapper.Map<List<EmployeeKPIResultDto>>(results);
        }
    }
}
