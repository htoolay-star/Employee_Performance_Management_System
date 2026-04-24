using EPMS.Domain.Contracts;
using EPMS.Domain.Data;
using EPMS.Domain.Interface.Irepo.Auth;
using EPMS.Domain.Repository.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Repository.Shared
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        private IAuthModule? _auth;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IAuthModule Auth => _auth ??= new AuthModule(_context);

        public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();
        public async Task BeginTransactionAsync() => await _context.Database.BeginTransactionAsync();
        public async Task CommitAsync() => await _context.Database.CommitTransactionAsync();
        public async Task RollbackAsync() => await _context.Database.RollbackTransactionAsync();

        public void Dispose() => _context.Dispose();
    }
}
