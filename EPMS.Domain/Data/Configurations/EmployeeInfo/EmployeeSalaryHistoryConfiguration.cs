using EPMS.Domain.Entities.EmployeeInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPMS.Domain.Data.Configurations.EmployeeInfo
{
    public class EmployeeSalaryHistoryConfiguration : IEntityTypeConfiguration<EmployeeSalaryHistory>
    {
        public void Configure(EntityTypeBuilder<EmployeeSalaryHistory> entity)
        {
            entity.ToTable("EmployeeSalaryHistories", "hr");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).UseIdentityColumn();

            entity.Property(e => e.PreviousAmount).HasColumnType("decimal(18,2)").IsRequired();
            entity.Property(e => e.NewAmount).HasColumnType("decimal(18,2)").IsRequired();
            entity.Property(e => e.EffectiveDate).IsRequired();
            entity.Property(e => e.ChangeReason).HasMaxLength(500).IsRequired();
            entity.Property(e => e.ApprovedAt);
            entity.Property(e => e.CreatedAt).IsRequired();

            entity.HasOne(e => e.Employee)
                  .WithMany()
                  .HasForeignKey(e => e.EmployeeId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.ApprovedBy)
                  .WithMany()
                  .HasForeignKey(e => e.ApprovedById)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => new { e.EmployeeId, e.EffectiveDate });
            entity.HasIndex(e => e.CreatedAt);
        }
    }
}
