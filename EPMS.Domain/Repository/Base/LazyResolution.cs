using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Repository.Base
{
    public class LazyResolution<T> : Lazy<T> where T : class
    {
        public LazyResolution(IServiceProvider sp)
            : base(() => sp.GetRequiredService<T>())
        {
        }
    }
}
