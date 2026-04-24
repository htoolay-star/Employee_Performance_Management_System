using EPMS.Domain.Data;
using EPMS.Domain.Entities.PerformanceReview;
using EPMS.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EPMS.Domain.Repositories.Implementations
{
    public class PerformanceReviewRepository : IPerformanceReviewRepository
    {
        private readonly AppDbContext _context;

        public PerformanceReviewRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(PerformanceReview review)
        {
            await _context.PerformanceReviews.AddAsync(review);
            await _context.SaveChangesAsync();
        }

        public async Task<List<PerformanceReview>> GetAllAsync()
        {
            return await _context.PerformanceReviews.ToListAsync();
        }

        public async Task<PerformanceReview?> GetByIdAsync(Guid id)
        {
            return await _context.PerformanceReviews.FindAsync(id);
        }
    }
}
