using System.Security.Claims;
using EPMS.Domain.Interface.IService.Auth;
using EPMS.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace EPMS.Api.Authorization;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public PermissionAuthorizationHandler(
        IHttpContextAccessor httpContextAccessor,
        IServiceScopeFactory serviceScopeFactory)
    {
        _httpContextAccessor = httpContextAccessor;
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        var user = _httpContextAccessor.HttpContext?.User;
        if (user == null)
        {
            context.Fail();
            return;
        }

        var roles = user.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
        if (roles.Contains(RoleConstants.Admin) || roles.Contains(RoleConstants.SystemAdmin))
        {
            context.Succeed(requirement);
            return;
        }

        var userIdClaim = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
        {
            context.Fail();
            return;
        }

        using var scope = _serviceScopeFactory.CreateScope();
        var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();

        var permissions = await authService.GetUserPermissionsAsync(userId);

        if (permissions.Contains(requirement.PermissionCode))
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }
    }
}
