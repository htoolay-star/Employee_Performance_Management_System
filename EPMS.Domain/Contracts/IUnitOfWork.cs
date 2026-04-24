using EPMS.Domain.Interface.Irepo.Auth;
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

        Task<int> CompleteAsync();
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
