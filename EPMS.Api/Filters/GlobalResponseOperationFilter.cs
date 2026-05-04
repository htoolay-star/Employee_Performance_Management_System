using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace EPMS.Api.Filters
{
    public class GlobalResponseOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (!operation.Responses.ContainsKey("400"))
                operation.Responses.Add("400", new OpenApiResponse { Description = "Bad Request (Validation Error)" });

            if (!operation.Responses.ContainsKey("500"))
                operation.Responses.Add("500", new OpenApiResponse { Description = "Internal Server Error" });

            var hasAuthorizeAttribute =
                context.MethodInfo.DeclaringType?.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any() == true ||
                context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();

            var hasAllowAnonymousAttribute =
                context.MethodInfo.GetCustomAttributes(true).OfType<AllowAnonymousAttribute>().Any();

            var isAuthorizedEndpoint = hasAuthorizeAttribute && !hasAllowAnonymousAttribute;

            if (isAuthorizedEndpoint)
            {
                if (!operation.Responses.ContainsKey("401"))
                    operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized (Invalid or missing token)" });

                if (!operation.Responses.ContainsKey("403"))
                    operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden (User does not have the required role)" });
            }
        }
    }
}
