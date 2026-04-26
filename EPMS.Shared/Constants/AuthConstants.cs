using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Shared.Constants
{
    public class AuthConstants
    {
        public static class AppRoles
        {
            public const string SystemAdmin = "SystemAdmin";
            public const string Admin = "Admin";
            public const string User = "User";

            public static readonly IReadOnlyList<string> AssignableRoles =
                    new[] { Admin, User };

            public static class Ids
            {
                public const int SystemAdmin = 1;
                public const int Admin = 2;
                public const int User = 3;
            }
        }
    }
}
