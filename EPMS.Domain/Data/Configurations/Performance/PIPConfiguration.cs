using EPMS.Domain.Entities.Performance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Data.Configurations.Performance
{
    public class PIPConfiguration : IEntityTypeConfiguration<PIP>
    {
        public void Configure(EntityTypeBuilder<PIP> builder)
        {
            builder.ToTable("PIPs", "perf");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).UseIdentityColumn();

            builder.Property(e => e.Status).HasMaxLength(20).HasDefaultValue("Open");
            builder.Property(e => e.Reason).IsRequired();

            builder.HasOne(e => e.Employee)
                   .WithMany()
                   .HasForeignKey(e => e.EmployeeId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Manager)
                   .WithMany()
                   .HasForeignKey(e => e.ManagerId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.OriginAppraisal)
                   .WithMany()
                   .HasForeignKey(e => e.AppraisalId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.Objectives)
                   .WithOne(o => o.PIP)
                   .HasForeignKey(o => o.PIPId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(e => e.CreatedAt).IsRequired();
            builder.Property(e => e.UpdatedAt).IsRequired();
            builder.Property(e => e.Version).IsRowVersion();
        }
    }
}
