using EPMS.Domain.Contracts;
using EPMS.Domain.Data;
using EPMS.Domain.Data.Interceptors;
using EPMS.Domain.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace EPMS.Api.Extensions
{
    public static class DatabaseExtensions
    {
        public static IServiceCollection AddDatabaseInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<AuditInterceptor>();

            services.AddDbContext<AppDbContext>((sp, options) =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"))
                       .AddInterceptors(sp.GetRequiredService<AuditInterceptor>());
            });

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Scan for specific Repositories using Scrutor
            services.Scan(scan => scan
                .FromAssembliesOf(typeof(UnitOfWork))
                .AddClasses(classes => classes.Where(t => t.Name.EndsWith("Repository")))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime()
            );

            return services;
        }
    }
}
