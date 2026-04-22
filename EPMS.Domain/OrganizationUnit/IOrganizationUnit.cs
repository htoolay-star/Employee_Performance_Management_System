using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.OrganizationUnit
{
    internal class IOrganizationUnit
    {
        Guid Id { get; set; }
        string Name { get; set; }
        string Code { get; set; }
    }
}
