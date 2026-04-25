using EPMS.Domain.Data;
using EPMS.Domain.Entities.Auth;
using EPMS.Domain.Interface.Irepo.Auth;
using EPMS.Domain.Repository.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Repository.Auth
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }

        public async Task<bool> ExistsAsync(string email) =>
            await _dbSet.AnyAsync(u => u.Email == email);

        public async Task<User?> GetByEmailAsync(string email) =>
            await _dbSet.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == email);
    }
}
