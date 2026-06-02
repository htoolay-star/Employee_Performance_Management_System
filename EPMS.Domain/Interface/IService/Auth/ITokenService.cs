using System.Security.Claims;

namespace EPMS.Domain.Interface.IService.Auth
{
    public interface ITokenService
    {
        public record TokenUserInfo(long Id, string Email, string Name, List<string> Roles, string JwtId, bool IsFirstLogin);
        string GenerateAccessToken(TokenUserInfo user);

        string GenerateRefreshToken();

        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
