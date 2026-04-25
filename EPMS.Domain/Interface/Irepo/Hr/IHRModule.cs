using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Interface.Irepo.Hr
{
    public interface IHRModule
    {
        IDepartmentRepository Departments { get; }
        ITeamRepository Teams { get; }
    }
}
