using EPMS.Domain.Interface.IService.App;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace EPMS.Domain.Services.App
{
    public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
    {
        public long? UserId
        {
            get
            {
                var userIdClaim = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
                return long.TryParse(userIdClaim, out var userId) ? userId : null;
            }
        }

        public string? Email => httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);

        public bool IsAuthenticated => httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

        public bool IsAdmin =>
            httpContextAccessor.HttpContext?.User?.IsInRole("Admin") == true ||
            httpContextAccessor.HttpContext?.User?.IsInRole("SystemAdmin") == true;
    }
}
