using EPMS.Domain.Entities.Audit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Data.Configurations.Audit
{
    public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.ToTable("AuditLogs", "audit");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).UseIdentityColumn();

            builder.Property(e => e.EntityName).HasMaxLength(100).IsRequired();
            builder.Property(e => e.EntityId).HasMaxLength(100).IsRequired();
            builder.Property(e => e.Action).HasMaxLength(50).IsRequired();

            builder.Property(e => e.OldValues).HasColumnType("nvarchar(max)");
            builder.Property(e => e.NewValues).HasColumnType("nvarchar(max)");

            builder.Property(e => e.IpAddress).HasMaxLength(50);

            builder.Property(e => e.Timestamp).IsRequired();

            builder.HasIndex(e => e.Timestamp)
                   .HasDatabaseName("IX_AuditLogs_Timestamp");

            builder.HasIndex(e => new { e.EntityName, e.EntityId });
            builder.HasIndex(e => e.UserId);

            builder.HasOne(e => e.User)
                   .WithMany()
                   .HasForeignKey(e => e.UserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
