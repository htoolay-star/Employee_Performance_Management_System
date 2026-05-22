using Microsoft.AspNetCore.Authorization;

namespace EPMS.Api.Authorization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class HasPermissionAttribute : AuthorizeAttribute
{
    public const string PolicyPrefix = "Permission_";

    public HasPermissionAttribute(string permissionCode)
    {
        Policy = $"{PolicyPrefix}{permissionCode}";
    }
}
