using AutoMapper;
using EPMS.Api.MappingProfiles;
using EPMS.Api.Middlewares;
using EPMS.Domain.Contracts;
using EPMS.Domain.Data;
using EPMS.Domain.Data.Interceptors;
using EPMS.Domain.Data.Seeding;
using EPMS.Domain.Interface.Irepo.Auth;
using EPMS.Domain.Interface.Irepo.Hr;
using EPMS.Domain.Interface.Irepo.Info;
using EPMS.Domain.Interface.IService.Auth;
using EPMS.Domain.Interfaces;
using EPMS.Domain.Repository.Auth;
using EPMS.Domain.Repository.Base;
using EPMS.Domain.Repository.Hr;
using EPMS.Domain.Repository.Info;
using EPMS.Domain.Services;
using EPMS.Domain.Services.Auth;
using EPMS.Shared.Enums.EPMS.Shared.Enums;
using EPMS.Shared.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<SeedSettings>(builder.Configuration.GetSection("SeedSettings"));
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddSingleton<AuditInterceptor>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>((sp, options) =>
{
    var auditInterceptor = sp.GetRequiredService<AuditInterceptor>();
    options.UseSqlServer(connectionString, x => x.MigrationsAssembly("EPMS.Api"))
           .AddInterceptors(auditInterceptor);
});

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(MappingProfile).Assembly));
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Dependency Injection (DI)

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IDbSeeder, DbSeeder>();

builder.Services.Scan(scan => scan
    .FromAssembliesOf(typeof(IPasswordHasher))

    // 1. Services registration
    .AddClasses(classes => classes.Where(t => t.Name.EndsWith("Service")))
        .AsImplementedInterfaces()
        .WithScopedLifetime()

    // 2. Repositories registration
    .AddClasses(classes => classes.Where(t => t.Name.EndsWith("Repository")))
        .AsImplementedInterfaces()
        .WithScopedLifetime()

    // 3. Hasher registration
    .AddClasses(classes => classes.Where(t => t.Name.EndsWith("Hasher")))
        .AsImplementedInterfaces()
        .WithSingletonLifetime()
);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseExceptionHandler();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<IDbSeeder>();
    await seeder.SeedAsync();
}

app.Run();
