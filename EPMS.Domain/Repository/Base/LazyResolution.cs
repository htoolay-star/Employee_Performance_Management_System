using Microsoft.Extensions.DependencyInjection;

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
