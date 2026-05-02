using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Audit;
using EPMS.Domain.Factories;
using EPMS.Domain.Interface.IService.App;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Data.Interceptors
{
    public sealed class AuditInterceptor(
    TimeProvider timeProvider,
    ICurrentUserService currentUserService,
    IAuditLogFactory auditLogFactory) : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(
            DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            if (eventData.Context is not null)
            {
                UpdateTimestamps(eventData.Context);
                AddAuditLogs(eventData.Context);
            }
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            if (eventData.Context is not null)
            {
                UpdateTimestamps(eventData.Context);
                AddAuditLogs(eventData.Context);
            }
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void UpdateTimestamps(DbContext context)
        {
            var entries = context.ChangeTracker.Entries<IAuditableEntity>();
            var utcNow = timeProvider.GetUtcNow();

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = utcNow;
                    entry.Entity.UpdatedAt = utcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = utcNow;
                }
            }
        }

        private void AddAuditLogs(DbContext context)
        {
            var auditableEntries = context.ChangeTracker
                .Entries<IAuditableEntity>()
                .Where(e => e.Entity is not AuditLog &&
                            (e.State == EntityState.Added ||
                             e.State == EntityState.Modified ||
                             e.State == EntityState.Deleted))
                .ToList();

            if (auditableEntries.Count == 0) return;

            var auditLogs = auditLogFactory.CreateAuditLogs(auditableEntries, currentUserService.UserId);

            context.Set<AuditLog>().AddRange(auditLogs);
        }
    }
}
