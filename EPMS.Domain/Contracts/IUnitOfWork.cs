using EPMS.Domain.Interface.Irepo.App;
using EPMS.Domain.Interface.Irepo.Auth;
using EPMS.Domain.Interface.Irepo.Hr;
using EPMS.Domain.Interface.Irepo.Info;
using EPMS.Domain.Interface.Irepo.Performance;
using EPMS.Domain.Interface.Irepo.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        IAppModule App { get; }
        IAuthModule Auth { get; }
        IInfoModule Info { get; }
        IHRModule HR { get; }
        IPerfModule Performance { get; }
        ISharedModule Shared { get; }

        Task<int> CompleteAsync();
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
