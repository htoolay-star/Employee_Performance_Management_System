using EPMS.Api.Extensions;
using EPMS.Api.Mapster;
using EPMS.Domain.Contracts;
using EPMS.Api.Jobs;
using Hangfire;
using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

MapsterConfig.Configure();

builder.Services
    .AddAppConfiguration(builder.Configuration)
    .AddDatabaseInfrastructure(builder.Configuration)
    .AddApplicationServices()
    .AddJwtAuthentication(builder.Configuration)
    .AddWebApi();

builder.Services.AddScoped<EmployeeImportJob>();
builder.Services.AddScoped<NightlyMaintenanceJob>();
builder.Services.AddHangfire(cfg => cfg
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHangfireServer(options =>
{
    options.WorkerCount = 1;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("BlazorPolicy", policy =>
    {
        policy.WithOrigins("https://localhost:7003", "http://localhost:5085")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("LoginPolicy", config =>
    {
        config.PermitLimit = 10;
        config.Window = TimeSpan.FromMinutes(15);
        config.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
        config.QueueLimit = 0;
    });

    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("BlazorPolicy");

app.UseRateLimiter();

app.UseHttpsRedirection();

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = [new EPMS.Api.Middlewares.HangfireDashboardAuthorizationFilter()]
});

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<IDbSeeder>();
    await seeder.SeedAsync();
}

// Register recurring jobs
RecurringJob.AddOrUpdate<NightlyMaintenanceJob>("nightly-maintenance",
    job => job.RunAsync(), "30 17 * * *"); // daily at 12:00 AM Myanmar (17:30 UTC)

app.Run();