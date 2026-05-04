using EPMS.Shared.DTOs.Common;
using EPMS.Shared.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EPMS.Api.Filters
{
    public class ValidationFilterAttribute : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                var response = SuccessResponse.Fail(
                    "Validation failed. Please check the inputted data.",
                    ErrorType.Validation,
                    errors
                );

                context.Result = new BadRequestObjectResult(response);
                return;
            }

            await next();
        }
    }
}
