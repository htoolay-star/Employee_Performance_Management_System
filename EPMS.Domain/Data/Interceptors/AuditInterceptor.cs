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
                var utcNow = timeProvider.GetUtcNow();
                ApplySoftDeleteLogic(eventData.Context, utcNow);
                UpdateTimestamps(eventData.Context, utcNow);
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
                var utcNow = timeProvider.GetUtcNow();
                ApplySoftDeleteLogic(eventData.Context, utcNow);
                UpdateTimestamps(eventData.Context, utcNow);
                AddAuditLogs(eventData.Context);
            }
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private static void UpdateTimestamps(DbContext context, DateTimeOffset utcNow)
        {
            foreach (var entry in context.ChangeTracker.Entries<IAuditableEntity>())
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

        private static void ApplySoftDeleteLogic(DbContext context, DateTimeOffset utcNow)
        {
            foreach (var entry in context.ChangeTracker.Entries<ISoftDeletable>())
            {
                if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    entry.Entity.IsDeleted = true;
                    entry.Entity.DeletedAt = utcNow;
                }
            }
        }
    }
}
