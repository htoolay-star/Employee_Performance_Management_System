using EPMS.Domain.Entities.Performance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPMS.Domain.Data.Configurations.Performance
{
    public class EntityKPIConfiguration : IEntityTypeConfiguration<EntityKPI>
    {
        public void Configure(EntityTypeBuilder<EntityKPI> entity)
        {
            entity.ToTable("EntityKPIs", "perf");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).UseIdentityColumn();

            entity.Property(e => e.PublicId).IsRequired();
            entity.HasIndex(e => e.PublicId).IsUnique().HasFilter("[IsDeleted] = 0");

            entity.Property(e => e.EntityType).HasMaxLength(50).IsRequired();
            entity.Property(e => e.EntityId).IsRequired();

            entity.HasIndex(e => new { e.EntityType, e.EntityId, e.KPIId })
                  .IsUnique()
                  .HasFilter("[IsDeleted] = 0");

            entity.Property(e => e.TargetValue).HasColumnType("decimal(18,4)");
            entity.Property(e => e.TargetUnit).HasMaxLength(100);

            entity.Property(e => e.Weightage).HasColumnType("decimal(5,2)").IsRequired();

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
