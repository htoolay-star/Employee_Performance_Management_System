using EPMS.Domain.Entities.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Interface.IService.Auth
{
    public interface ITokenService
    {
        public record TokenUserInfo(long Id, string Email, List<string> Roles);
        string GenerateAccessToken(TokenUserInfo user);

        string GenerateRefreshToken();
    }
}
