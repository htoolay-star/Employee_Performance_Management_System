using EPMS.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Shared.Constants
{
    public static class RoleConstants
    {
        public const string SystemAdmin = nameof(UserRole.SystemAdmin);
        public const string Admin = nameof(UserRole.Admin);
        public const string Staff = nameof(UserRole.User);

        public const string SA_Admin = "SystemAdmin,Admin";
    }
}
