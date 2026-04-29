using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Interface.Irepo.Shared
{
    public interface ISharedModule
    {
        ICategoryRepository Categories { get; }
        ITagRepository Tags { get; }
    }
}
