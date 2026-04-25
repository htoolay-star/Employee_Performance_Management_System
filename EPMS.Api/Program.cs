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
using EPMS.Domain.Interfaces;
using EPMS.Domain.Repository.Auth;
using EPMS.Domain.Repository.Base;
using EPMS.Domain.Repository.Hr;
using EPMS.Domain.Repository.Info;
using EPMS.Domain.Services;
using EPMS.Shared.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<SeedSettings>(builder.Configuration.GetSection("SeedSettings"));
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddSingleton<AuditInterceptor>();

builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssemblies(typeof(AppDbContext).Assembly);
});


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>((sp, options) =>
{
    var auditInterceptor = sp.GetRequiredService<AuditInterceptor>();
    options.UseSqlServer(connectionString, x => x.MigrationsAssembly("EPMS.Api"))
           .AddInterceptors(auditInterceptor);
});


builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(MappingProfile).Assembly));
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Dependency Injection (DI)

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IDbSeeder, DbSeeder>();

// Auth
builder.Services.AddScoped<IAuthModule, AuthModule>();
builder.Services.AddScoped<IInfoModule, InfoModule>();
builder.Services.AddScoped<IHRModule, HRModule>();

builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<ITeamService, TeamService>();

builder.Services.AddControllers();
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
