using EPMS.Api.Filters;
using EPMS.Api.Middlewares;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace EPMS.Api.Extensions
{
    public static class WebApiExtensions
    {
        public static IServiceCollection AddWebApi(this IServiceCollection services)
        {
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();

            services.AddControllers(options =>
            {
                options.Filters.Add<MustChangePasswordFilter>();
                options.Filters.Add<ValidationFilterAttribute>(); 
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            // The critical step to make custom validation work
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddSwaggerGen(c =>
            {
                c.OperationFilter<GlobalResponseOperationFilter>();
            });

            return services;
        }
    }
}
