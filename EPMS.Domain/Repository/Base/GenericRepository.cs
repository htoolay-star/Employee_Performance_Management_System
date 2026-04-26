using EPMS.Domain.Contracts;
using EPMS.Domain.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EPMS.Domain.Repository.Base
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<T?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
            => await _dbSet.FindAsync(new object[] { id }, cancellationToken);

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
            => await _dbSet.AsNoTracking().ToListAsync(cancellationToken);

        public async Task<(IEnumerable<T> Items, int TotalCount)> GetPagedListAsync(
            int page,
            int pageSize,
            Expression<Func<T, object>> orderBy,
            bool descending = false,
            CancellationToken cancellationToken = default)
        {
            var totalCount = await _dbSet.CountAsync(cancellationToken);

            var query = _dbSet.AsNoTracking();

            query = descending ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        public async Task<T?> FindAsync(
            Expression<Func<T, bool>> predicate,
            bool trackChanges = true,
            CancellationToken cancellationToken = default,
            params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = trackChanges ? _dbSet : _dbSet.AsNoTracking();

            if (includes != null && includes.Length > 0)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            return await query.FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public async Task<IEnumerable<T>> FindAllAsync(
            Expression<Func<T, bool>> predicate,
            bool trackChanges = false,
            CancellationToken cancellationToken = default,
            params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = trackChanges ? _dbSet.Where(predicate) : _dbSet.AsNoTracking().Where(predicate);

            if (includes != null && includes.Length > 0)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            return await query.ToListAsync(cancellationToken);
        }

        public void Add(T entity) => _dbSet.Add(entity);

        public void Update(T entity) => _dbSet.Update(entity);

        public void Delete(T entity) => _dbSet.Remove(entity);

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
            => await _dbSet.AnyAsync(predicate, cancellationToken);
    }

}

