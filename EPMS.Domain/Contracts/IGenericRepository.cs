using System.Linq.Expressions;

namespace EPMS.Domain.Contracts
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(object id, CancellationToken cancellationToken = default);

        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<T?> FindAsync(
            Expression<Func<T, bool>> predicate,
            bool trackChanges = true,
            CancellationToken cancellationToken = default,
            params Expression<Func<T, object>>[] includes);

        Task<IEnumerable<T>> FindAllAsync(
            Expression<Func<T, bool>> predicate,
            bool trackChanges = false,
            CancellationToken cancellationToken = default,
            params Expression<Func<T, object>>[] includes);

        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> GetAllDeletedAsync(CancellationToken cancellationToken = default);
        Task<T?> GetByIdDeletedAsync(object id, CancellationToken cancellationToken = default);
    }
}
