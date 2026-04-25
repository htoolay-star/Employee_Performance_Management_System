using EPMS.Domain.Contracts;
using EPMS.Domain.Data;
using EPMS.Domain.Interface.Irepo.Auth;
using EPMS.Domain.Interface.Irepo.Hr;
using EPMS.Domain.Interface.Irepo.Info;
using EPMS.Domain.Repository.Auth;
using EPMS.Domain.Repository.Hr;
using EPMS.Domain.Repository.Info;
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
        private IInfoModule? _info;
        private IHRModule? _hr;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IAuthModule Auth => _auth ??= new AuthModule(_context);
        public IInfoModule Info => _info ??= new InfoModule(_context);
        public IHRModule HR => _hr ??= new HRModule(_context);

        public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();
        public async Task BeginTransactionAsync() => await _context.Database.BeginTransactionAsync();
        public async Task CommitAsync() => await _context.Database.CommitTransactionAsync();
        public async Task RollbackAsync() => await _context.Database.RollbackTransactionAsync();

        public void Dispose() => _context.Dispose();
    }
}
