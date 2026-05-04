using EPMS.Domain.Entities.EmployeeInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPMS.Domain.Data.Configurations.EmployeeInfo
{
    public class EmployeeEmploymentHistoryConfiguration : IEntityTypeConfiguration<EmployeeEmploymentHistory>
    {
        public void Configure(EntityTypeBuilder<EmployeeEmploymentHistory> entity)
        {
            entity.ToTable("EmployeeEmploymentHistories", "hr");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).UseIdentityColumn();

            entity.Property(e => e.EmploymentStatus).HasMaxLength(50).IsRequired();
            entity.Property(e => e.EffectiveDate).IsRequired();
            entity.Property(e => e.EndDate);
            entity.Property(e => e.ChangeReason).HasMaxLength(500);
            entity.Property(e => e.CreatedAt).IsRequired();

            entity.HasOne(e => e.Employee)
                  .WithMany()
                  .HasForeignKey(e => e.EmployeeId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Department)
                  .WithMany()
                  .HasForeignKey(e => e.DepartmentId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Position)
                  .WithMany()
                  .HasForeignKey(e => e.PositionId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Manager)
                  .WithMany()
                  .HasForeignKey(e => e.ManagerId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.ChangedBy)
                  .WithMany()
                  .HasForeignKey(e => e.ChangedById)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => new { e.EmployeeId, e.EffectiveDate });
            entity.HasIndex(e => e.EndDate);
        }
    }
}
