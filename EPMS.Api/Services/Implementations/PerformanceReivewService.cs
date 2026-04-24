using AutoMapper;
using EPMS.Api.Services.Interfaces;
using EPMS.Domain.Entities.PerformanceReview;
using EPMS.Domain.Repositories.Interfaces;
using EPMS.Shared.DTOs;

public class PerformanceReviewService : IPerformanceReviewService
{
    private readonly IPerformanceReviewRepository _repo;
    private readonly IMapper _mapper;

    public PerformanceReviewService(
        IPerformanceReviewRepository repo,
        IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<PerformanceReviewDto> CreateAsync(CreatePerformanceReivewDto dto)
    {
        var entity = _mapper.Map<PerformanceReview>(dto);

        await _repo.AddAsync(entity);

        return _mapper.Map<PerformanceReviewDto>(entity);
    }

    public async Task<List<PerformanceReviewDto>> GetAllAsync()
    {
        var list = await _repo.GetAllAsync();
        return _mapper.Map<List<PerformanceReviewDto>>(list);
    }

    public async Task<PerformanceReviewDto?> GetByIdAsync(Guid id)
    {
        var entity = await _repo.GetByIdAsync(id);
        return entity == null ? null : _mapper.Map<PerformanceReviewDto>(entity);
    }
}
