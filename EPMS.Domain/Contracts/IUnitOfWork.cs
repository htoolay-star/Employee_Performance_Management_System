using EPMS.Domain.Interface.Irepo.Auth;
using EPMS.Domain.Interface.Irepo.Hr;
using EPMS.Domain.Interface.Irepo.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Contracts
{
    public interface IUnitOfWork
    {
        IAuthModule Auth { get; }
        IInfoModule Info { get; }
        IHRModule HR { get; }

        Task<int> CompleteAsync();
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
        Task SaveChangesAsync(CancellationToken ct);
    }
}
