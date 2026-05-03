using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Shared.Enums
{
    public enum UserRole
    {
        /// <summary> Technical support & Emergency troubleshooting only </summary>
        SystemAdmin = 1,

        /// <summary> Power user for HR & Operations (No Role assignment) </summary>
        Admin = 2,

        /// <summary> Standard employee access </summary>
        User = 3
    }
}
