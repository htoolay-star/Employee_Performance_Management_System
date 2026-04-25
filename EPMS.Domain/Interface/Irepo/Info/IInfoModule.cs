using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Interface.Irepo.Info
{
    public interface IInfoModule
    {
        IEmployeeProfileRepository EmployeeProfiles { get; }
    }
}
