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

        public async Task<IEnumerable<T>> GetAllDeletedAsync(CancellationToken cancellationToken = default)
        {
            if (!typeof(ISoftDeletable).IsAssignableFrom(typeof(T)))
                return Enumerable.Empty<T>();

            var param = Expression.Parameter(typeof(T), "e");
            var prop = Expression.Property(param, nameof(ISoftDeletable.IsDeleted));
            var body = Expression.Equal(prop, Expression.Constant(true));
            var predicate = Expression.Lambda<Func<T, bool>>(body, param);

            return await _dbSet.IgnoreQueryFilters().Where(predicate).ToListAsync(cancellationToken);
        }

        public async Task<T?> GetByIdDeletedAsync(object id, CancellationToken cancellationToken = default)
        {
            if (!typeof(ISoftDeletable).IsAssignableFrom(typeof(T)))
                return await GetByIdAsync(id, cancellationToken);

            var param = Expression.Parameter(typeof(T), "e");
            var idProp = Expression.Property(param, "Id");
            var idBody = Expression.Equal(idProp, Expression.Constant(Convert.ToInt64(id)));
            var idPredicate = Expression.Lambda<Func<T, bool>>(idBody, param);

            return await _dbSet.IgnoreQueryFilters().FirstOrDefaultAsync(idPredicate, cancellationToken);
        }
    }

}

