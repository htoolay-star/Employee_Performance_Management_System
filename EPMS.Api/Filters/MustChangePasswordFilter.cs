using EPMS.Shared.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace EPMS.Api.Filters
{
    public class MustChangePasswordFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.HttpContext.User.Identity?.IsAuthenticated == true)
            {
                var isFirstLoginStr = context.HttpContext.User.FindFirstValue(AppClaims.IsFirstLogin);

                if (bool.TryParse(isFirstLoginStr, out bool isFirst) && isFirst)
                {
                    var actionName = context.ActionDescriptor.DisplayName;

                    if (!actionName!.Contains("ChangePassword"))
                    {
                        context.Result = new ObjectResult(new
                        {
                            message = "You must change your default password before accessing this resource."
                        })
                        { StatusCode = StatusCodes.Status403Forbidden };
                        return;
                    }
                }
            }
            await next();
        }
    }
}
