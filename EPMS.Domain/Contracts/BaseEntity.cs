using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Contracts
{
    public abstract class BaseEntity
    {
        public long Id { get; set; }
    }
}
