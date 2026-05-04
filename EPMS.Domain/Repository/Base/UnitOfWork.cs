using EPMS.Domain.Contracts;
using EPMS.Domain.Data;
using EPMS.Domain.Interface.Irepo.App;
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

        private readonly Lazy<IAppModule> _app;
        private readonly Lazy<IAuthModule> _auth;
        private readonly Lazy<IInfoModule> _info;
        private readonly Lazy<IHRModule> _hr;
        private readonly Lazy<IPerfModule> _perf;
        private readonly Lazy<ISharedModule> _shared;

        public UnitOfWork(
            AppDbContext context,
            Lazy<IAppModule> app,
            Lazy<IAuthModule> auth,
            Lazy<IInfoModule> info,
            Lazy<IHRModule> hr,
            Lazy<IPerfModule> perf,
            Lazy<ISharedModule> shared
            )
        {
            _context = context;
            _app = app;
            _auth = auth;
            _info = info;
            _hr = hr;
            _perf = perf;
            _shared = shared;
        }

        public IAppModule App => _app.Value;
        public IAuthModule Auth => _auth.Value;
        public IInfoModule Info => _info.Value;
        public IHRModule HR => _hr.Value;
        public IPerfModule Performance => _perf.Value;
        public ISharedModule Shared => _shared.Value;

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