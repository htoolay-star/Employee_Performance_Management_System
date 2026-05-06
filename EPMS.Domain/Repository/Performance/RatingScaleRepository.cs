using EPMS.Domain.Data;
using EPMS.Domain.Entities.Hr;
using EPMS.Domain.Interface.Irepo.Performance;
using EPMS.Domain.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace EPMS.Domain.Repository.Performance;

public class RatingScaleRepository : GenericRepository<RatingScale>, IRatingScaleRepository
{
    public RatingScaleRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<RatingScale>> GetActiveAsync()
    {
        return await _dbSet
            .Where(rs => rs.IsActive && !rs.IsDeleted)
            .OrderBy(rs => rs.Rating)
            .ToListAsync();
    }

    public async Task<RatingScale?> GetByRatingAsync(int rating)
    {
        return await _dbSet
            .FirstOrDefaultAsync(rs => rs.Rating == rating && !rs.IsDeleted);
    }

    public async Task<bool> RatingExistsAsync(int rating)
    {
        return await _dbSet.AnyAsync(rs => rs.Rating == rating && !rs.IsDeleted);
    }

    public async Task<bool> RatingExistsAsync(int rating, long excludeId)
    {
        return await _dbSet.AnyAsync(rs => rs.Rating == rating && rs.Id != excludeId && !rs.IsDeleted);
    }
}
