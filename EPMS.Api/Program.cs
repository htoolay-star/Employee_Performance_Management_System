using EPMS.Domain.Data;
using EPMS.Domain.Data.Interceptors;
using EPMS.Domain.Interface.Irepo;
using EPMS.Domain.Interfaces;
using EPMS.Domain.Services;
using EPMS.Domain.Repository;
using Microsoft.EntityFrameworkCore;
using MediatR;
using AutoMapper;
using EPMS.Domain.Contracts;

var builder = WebApplication.CreateBuilder(args);


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


var mapperConfig = new AutoMapper.MapperConfiguration(mc =>
{
    mc.AddMaps(typeof(Program).Assembly); 
});

AutoMapper.IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
// 5. Dependency Injection (DI)
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
