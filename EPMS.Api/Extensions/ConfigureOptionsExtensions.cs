using EPMS.Shared.Models;

namespace EPMS.Api.Extensions
{
    public static class ConfigureOptionsExtensions
    {
        public static IServiceCollection AddAppConfiguration(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<SeedSettings>(config.GetSection("SeedSettings"));
            services.Configure<JwtSettings>(config.GetSection("JwtSettings"));
            services.Configure<CryptoSettings>(config.GetSection("CryptoSettings"));
            services.Configure<CacheSettings>(config.GetSection("CacheSettings"));

            return services;
        }
    }
}
