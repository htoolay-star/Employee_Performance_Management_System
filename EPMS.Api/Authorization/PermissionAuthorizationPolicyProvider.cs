using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace EPMS.Api.Authorization;

public class PermissionAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
{
    public PermissionAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
        : base(options) { }

    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (policyName.StartsWith(HasPermissionAttribute.PolicyPrefix, StringComparison.OrdinalIgnoreCase))
        {
            var permissionCode = policyName[HasPermissionAttribute.PolicyPrefix.Length..];
            var requirement = new PermissionRequirement(permissionCode);
            return new AuthorizationPolicyBuilder()
                .AddRequirements(requirement)
                .Build();
        }

        return await base.GetPolicyAsync(policyName);
    }
}
