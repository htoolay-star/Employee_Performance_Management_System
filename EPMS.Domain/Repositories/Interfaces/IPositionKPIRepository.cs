using EPMS.Domain.Entities.Performance;

public interface IPositionKPIRepository
{
    Task<List<PositionKPI>> GetByPositionIdAsync(long positionId);

    Task<PositionKPI?> GetByIdAsync(long id);

    Task AddAsync(PositionKPI entity);

    Task<decimal> GetTotalWeightByPositionAsync(long positionId);

    Task SaveChangesAsync();
}