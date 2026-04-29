using EPMS.Domain.Entities.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Interface.IService.Auth
{
    public interface ITokenService
    {
        public record TokenUserInfo(long Id, string Email, List<string> Roles, string JwtId, bool IsFirstLogin);
        string GenerateAccessToken(TokenUserInfo user);

        string GenerateRefreshToken();

        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
