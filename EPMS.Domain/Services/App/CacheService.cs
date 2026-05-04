using EPMS.Domain.Interface.IService.App;
using EPMS.Shared.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPMS.Domain.Services.App
{
    public class CacheService(IDistributedCache cache, IOptions<CacheSettings> cacheSettings) : ICacheService
    {
        private readonly CacheSettings _settings = cacheSettings.Value;
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles,
            PropertyNameCaseInsensitive = true
        };

        public async Task SetAsync<T>(string key, T data, TimeSpan? absoluteExpireTime = null, CancellationToken cancellationToken = default)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromMinutes(_settings.DefaultExpirationMinutes)
            };

            var jsonData = JsonSerializer.Serialize(data, _jsonOptions);

            await cache.SetStringAsync(key, jsonData, options, cancellationToken);
        }

        public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            var jsonData = await cache.GetStringAsync(key, cancellationToken);

            if (string.IsNullOrEmpty(jsonData))
                return default;

            return JsonSerializer.Deserialize<T>(jsonData, _jsonOptions);
        }

        public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            await cache.RemoveAsync(key, cancellationToken);
        }
    }
}
