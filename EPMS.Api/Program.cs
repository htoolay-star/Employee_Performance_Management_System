using EPMS.Api.Services.Interfaces;
using EPMS.Domain.Data;
using EPMS.Domain.Data.Interceptors;
using EPMS.Domain.Repositories.Implementations;
using EPMS.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AppDbContext).Assembly));
builder.Services.AddSingleton<AuditInterceptor>();

// auto mapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Repositories
builder.Services.AddScoped<IPerformanceReviewRepository, PerformanceReviewRepository>();

// Services
builder.Services.AddScoped<IPerformanceReviewService, PerformanceReviewService>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>((sp, options) =>
{
    var auditInterceptor = sp.GetRequiredService<AuditInterceptor>();
    options.UseSqlServer(connectionString)
           .AddInterceptors(auditInterceptor);
});

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();