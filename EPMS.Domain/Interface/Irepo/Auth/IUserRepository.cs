using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Interface.Irepo.Auth
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<bool> ExistsAsync(string email);
        Task<User?> GetByEmailAsync(string email, bool trackChanges = false);
    }
}
