using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Hr;

namespace EPMS.Domain.Interface.Irepo.Performance;

public interface IRatingScaleRepository : IGenericRepository<RatingScale>
{
    Task<IEnumerable<RatingScale>> GetActiveAsync();
    Task<RatingScale?> GetByRatingAsync(int rating);
    Task<bool> RatingExistsAsync(int rating);
    Task<bool> RatingExistsAsync(int rating, long excludeId);
}
