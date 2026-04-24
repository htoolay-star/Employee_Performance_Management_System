using EPMS.Domain.Data;
using EPMS.Domain.Interface.Irepo.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Repository.Auth
{
    public class AuthModule : IAuthModule
    {
        private readonly AppDbContext _context;
        private IUserRepository? _users;
        private IRoleRepository? _roles;

        public AuthModule(AppDbContext context) => _context = context;

        public IUserRepository Users => _users ??= new UserRepository(_context);
        public IRoleRepository Roles => _roles ??= new RoleRepository(_context);
    }
}
