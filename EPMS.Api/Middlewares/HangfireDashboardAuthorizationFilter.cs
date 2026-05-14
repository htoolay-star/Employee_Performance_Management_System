using EPMS.Shared.Constants;
using Hangfire.Dashboard;
using System.Net;

namespace EPMS.Api.Middlewares;

public class HangfireDashboardAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        var httpContext = context.GetHttpContext();
        var isLocal = httpContext.Connection.RemoteIpAddress == null
            || IPAddress.IsLoopback(httpContext.Connection.RemoteIpAddress);
        if (isLocal) return true;

        return httpContext.User.Identity?.IsAuthenticated == true
            && httpContext.User.IsInRole(RoleConstants.SystemAdmin);
    }
}
