using EPMS.Domain.Entities.Performance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPMS.Domain.Data.Configurations.Performance
{
    public class EmployeeKPIHistoryConfiguration : IEntityTypeConfiguration<EmployeeKPIHistory>
    {
        public void Configure(EntityTypeBuilder<EmployeeKPIHistory> entity)
        {
            entity.ToTable("EmployeeKPIHistories", "perf");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).UseIdentityColumn();

            entity.Property(e => e.EmployeeId).IsRequired();
            entity.Property(e => e.CycleId).IsRequired();
            entity.Property(e => e.SnapshotDate).IsRequired();

            entity.Property(e => e.TargetValue).HasMaxLength(100);
            entity.Property(e => e.TargetUnit).HasMaxLength(100);
            entity.Property(e => e.Weightage).HasColumnType("decimal(5,2)").IsRequired();

            entity.HasOne(e => e.Cycle)
                  .WithMany()
                  .HasForeignKey(e => e.CycleId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.KPI)
                  .WithMany()
                  .HasForeignKey(e => e.KPIId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Priority)
                  .WithMany()
                  .HasForeignKey(e => e.PriorityId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => new { e.EmployeeId, e.CycleId });
            entity.HasIndex(e => e.CycleId);
        }
    }
}
