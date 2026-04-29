using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Interface.Irepo.Performance
{
    public interface IPerfModule
    {
         IAppraisalRepository Appraisals { get; }
    }
}
