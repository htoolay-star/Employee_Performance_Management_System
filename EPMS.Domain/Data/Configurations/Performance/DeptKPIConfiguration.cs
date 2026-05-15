using EPMS.Domain.Entities.Performance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPMS.Domain.Data.Configurations.Performance
{
    public class DeptKPIConfiguration : IEntityTypeConfiguration<DeptKPI>
    {
        public void Configure(EntityTypeBuilder<DeptKPI> entity)
        {
            entity.ToTable("DeptKPIs", "perf");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).UseIdentityColumn();

            entity.Property(e => e.PublicId).IsRequired();
            entity.HasIndex(e => e.PublicId).IsUnique().HasFilter("[IsDeleted] = 0");

            entity.HasIndex(e => new { e.DeptId, e.KPIId })
                  .IsUnique()
                  .HasFilter("[IsDeleted] = 0");

            entity.Property(e => e.TargetValue).HasMaxLength(100);
            entity.Property(e => e.TargetUnit).HasMaxLength(100);

            entity.Property(e => e.Weightage).HasColumnType("decimal(5,2)").IsRequired();

            entity.HasOne(e => e.Department)
                  .WithMany(d => d.DeptKPIs)
                  .HasForeignKey(e => e.DeptId)
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
