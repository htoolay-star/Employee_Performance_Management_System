using EPMS.Domain.Interface.Irepo.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Interface.Irepo.Auth
{
    public interface IAuthModule
    {
        IUserRepository Users { get; }
        IUserRefreshTokenRepository UsersRefreshToken { get; }
        IRoleRepository Roles { get; }
        IPermissionRepository Permissions { get; }
    }
}
