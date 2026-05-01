using EPMS.Domain.Contracts;
using EPMS.Domain.Data;
using EPMS.Domain.Interface.Irepo.Auth;
using EPMS.Domain.Interface.Irepo.Hr;
using EPMS.Domain.Interface.Irepo.Info;
using EPMS.Domain.Interface.Irepo.Performance;
using EPMS.Domain.Interface.Irepo.Shared;
using EPMS.Domain.Repository.Auth;
using EPMS.Domain.Repository.Hr;
using EPMS.Domain.Repository.Info;
using EPMS.Domain.Repository.Performance;
using EPMS.Domain.Repository.Shared;
using Microsoft.EntityFrameworkCore.Storage;

namespace EPMS.Domain.Repository.Base
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction? _transaction;
       
        private IAuthModule? _auth;
        private IInfoModule? _info;
        private IHRModule? _hr;
        private IPerfModule? _perf;
        private ISharedModule? _shared;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IAuthModule Auth => _auth ??= new AuthModule(_context);
        public IInfoModule Info => _info ??= new InfoModule(_context);
        public IHRModule HR => _hr ??= new HRModule(_context);
        public IPerfModule Performance => _perf ??= new PerfModule(_context);
        public ISharedModule Shared => _shared ??= new SharedModule(_context);

        public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                if (_transaction != null) await _transaction.CommitAsync();
            }
            catch
            {
                await RollbackAsync();
                throw;
            }
            finally
            {
                if (_transaction != null)
                {
                    _transaction.Dispose();
                    _transaction = null;
                }
            }
        }

        public async Task RollbackAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                _transaction.Dispose();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}