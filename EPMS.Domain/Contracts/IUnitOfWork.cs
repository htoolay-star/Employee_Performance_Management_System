using EPMS.Domain.Interface.Irepo.App;
using EPMS.Domain.Interface.Irepo.Auth;
using EPMS.Domain.Interface.Irepo.Hr;
using EPMS.Domain.Interface.Irepo.Info;
using EPMS.Domain.Interface.Irepo.Performance;
using EPMS.Domain.Interface.Irepo.Shared;

namespace EPMS.Domain.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        IAppModule App { get; }
        IAuthModule Auth { get; }
        IInfoModule Info { get; }
        IHRModule HR { get; }
        IPerfModule Perf { get; }
        ISharedModule Shared { get; }

        Task<int> CompleteAsync();
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
