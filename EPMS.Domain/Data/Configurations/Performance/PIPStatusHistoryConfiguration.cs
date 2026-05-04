using EPMS.Domain.Entities.Performance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPMS.Domain.Data.Configurations.Performance
{
    public class PIPStatusHistoryConfiguration : IEntityTypeConfiguration<PIPStatusHistory>
    {
        public void Configure(EntityTypeBuilder<PIPStatusHistory> entity)
        {
            entity.ToTable("PIPStatusHistories", "perf");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).UseIdentityColumn();

            entity.Property(e => e.FromStatus).HasMaxLength(50).IsRequired();
            entity.Property(e => e.ToStatus).HasMaxLength(50).IsRequired();
            entity.Property(e => e.ChangedAt).IsRequired();
            entity.Property(e => e.Reason).HasMaxLength(500);

            entity.HasOne(e => e.PIP)
                  .WithMany()
                  .HasForeignKey(e => e.PIPId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.ChangedBy)
                  .WithMany()
                  .HasForeignKey(e => e.ChangedById)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => new { e.PIPId, e.ChangedAt });
        }
    }
}
