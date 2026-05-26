namespace EPMS.Domain.Interface.IService.App
{
    public interface ICacheService
    {
        Task<T?> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan? absoluteExpireTime = null, CancellationToken cancellationToken = default);
        Task SetAsync<T>(string key, T data, TimeSpan? absoluteExpireTime = null, CancellationToken cancellationToken = default);
        Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default);
        Task RemoveAsync(string key, CancellationToken cancellationToken = default);
    }
}
