using EPMS.Domain.Data;
using EPMS.Domain.Entities.Performance;
using EPMS.Domain.Interface.Irepo.Performance;
using EPMS.Domain.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace EPMS.Domain.Repository.Performance
{
    public class FormTemplateRepository : GenericRepository<FormTemplate>, IFormTemplateRepository
    {
        public FormTemplateRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<FormTemplate>> GetAllWithQuestionsAsync()
        {
            return await _dbSet
                .Where(x => !x.IsDeleted)
                .Include(x => x.Questions)
                .OrderBy(x => x.Name)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<FormTemplate>> GetActiveAsync()
        {
            return await _dbSet
                .Where(x => x.IsActive && !x.IsDeleted)
                .Include(x => x.Questions)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<FormTemplate?> GetByNameAsync(string name)
        {
            return await _dbSet
                .FirstOrDefaultAsync(x => x.Name == name && !x.IsDeleted);
        }

        public async Task<bool> NameExistsAsync(string name, long? excludeId = null)
        {
            return await _dbSet
                .AnyAsync(x => x.Name == name && !x.IsDeleted && (excludeId == null || x.Id != excludeId));
        }
    }
}