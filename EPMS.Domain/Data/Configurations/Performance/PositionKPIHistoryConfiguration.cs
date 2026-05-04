using EPMS.Domain.Entities.Performance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPMS.Domain.Data.Configurations.Performance
{
    public class PositionKPIHistoryConfiguration : IEntityTypeConfiguration<PositionKPIHistory>
    {
        public void Configure(EntityTypeBuilder<PositionKPIHistory> entity)
        {
            entity.ToTable("PositionKPIHistories", "perf");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).UseIdentityColumn();

            entity.Property(e => e.Weightage).HasColumnType("decimal(5,2)").IsRequired();
            entity.Property(e => e.TargetValue).HasMaxLength(100);
            entity.Property(e => e.TargetUnit).HasMaxLength(50);
            entity.Property(e => e.EffectiveDate).IsRequired();
            entity.Property(e => e.EndDate);
            entity.Property(e => e.ChangeReason).HasMaxLength(500);
            entity.Property(e => e.CreatedAt).IsRequired();

            entity.HasOne(e => e.Position)
                  .WithMany()
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

            entity.HasOne(e => e.ChangedBy)
                  .WithMany()
                  .HasForeignKey(e => e.ChangedById)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => new { e.PositionId, e.EffectiveDate });
            entity.HasIndex(e => new { e.KPIId, e.EffectiveDate });
            entity.HasIndex(e => e.EndDate);
        }
    }
}
