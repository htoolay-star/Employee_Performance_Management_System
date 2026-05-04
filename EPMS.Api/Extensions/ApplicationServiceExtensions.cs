using EPMS.Api.MappingProfiles;
using EPMS.Domain.Contracts;
using EPMS.Domain.Data.Seeding;
using EPMS.Domain.Factories;
using EPMS.Domain.Repository.Base;
using EPMS.Shared.Validators;
using FluentValidation;
using System.Reflection;

namespace EPMS.Api.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton(TimeProvider.System);
            services.AddDistributedMemoryCache();
            services.AddHttpContextAccessor();

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(typeof(ValidationExtensions).Assembly);
            services.AddAutoMapper(cfg => cfg.AddMaps(typeof(MappingProfile).Assembly));

            services.AddTransient(typeof(Lazy<>), typeof(LazyResolution<>));
            services.AddTransient<IDbSeeder, DbSeeder>();
            services.AddSingleton<IAuditLogFactory, AuditLogFactory>();

            services.Scan(scan => scan
                .FromAssembliesOf(typeof(UnitOfWork))
                .AddClasses(classes => classes.Where(t => t.Name.EndsWith("Service") || t.Name.EndsWith("Module")))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime()
                .AddClasses(classes => classes.Where(t => t.Name.EndsWith("Hasher") || t.Name.EndsWith("TokenService")))
                    .AsImplementedInterfaces()
                    .WithSingletonLifetime()
            );

            return services;
        }
    }
}
