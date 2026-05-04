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
    public class PositionKPIConfiguration : IEntityTypeConfiguration<PositionKPI>
    {
        public void Configure(EntityTypeBuilder<PositionKPI> entity)
        {
            entity.ToTable("PositionKPIs", "perf");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).UseIdentityColumn();

            entity.Property(e => e.PublicId).IsRequired();
            entity.HasIndex(e => e.PublicId).IsUnique().HasFilter("[IsDeleted] = 0");

            entity.HasIndex(e => new { e.PositionId, e.KPIId })
                  .IsUnique()
                  .HasFilter("[IsDeleted] = 0");

            entity.Property(e => e.TargetValue).HasMaxLength(100);
            entity.Property(e => e.TargetUnit).HasMaxLength(100);

            entity.Property(e => e.Weightage).HasColumnType("decimal(5,2)").IsRequired();

            entity.HasOne(e => e.Position)
                  .WithMany(p => p.PositionKPIs)
                  .HasForeignKey(e => e.PositionId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.KPI)
                  .WithMany()
                  .HasForeignKey(e => e.KPIId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Priority)
                  .WithMany()
                  .HasForeignKey(e => e.PriorityId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.UpdatedAt).IsRequired();
            entity.Property(e => e.Version).IsRowVersion();

            entity.Property(e => e.IsDeleted).HasDefaultValue(false).IsRequired();
            entity.Property(e => e.DeletedAt);
        }
    }
}
