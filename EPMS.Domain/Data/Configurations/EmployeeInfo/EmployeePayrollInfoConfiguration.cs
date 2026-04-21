using EPMS.Domain.Entities.EmployeeInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Data.Configurations.EmployeeInfo
{
    public class EmployeePayrollInfoConfiguration : IEntityTypeConfiguration<EmployeePayrollInfo>
    {
        public void Configure(EntityTypeBuilder<EmployeePayrollInfo> entity)
        {
            entity.ToTable("EmployeePayrollInfo", "hr");
            entity.HasKey(e => e.EmployeeId);

            entity.HasOne(e => e.Profile)
                  .WithOne(p => p.PayrollInfo)
                  .HasForeignKey<EmployeePayrollInfo>(e => e.EmployeeId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.Salary).HasColumnType("decimal(18,2)").IsRequired();

            entity.Property(e => e.DateOfPayTypeChanged).HasColumnType("date");

            entity.Property(e => e.CostAllocate).HasMaxLength(100);
            entity.Property(e => e.PayByBacklog).HasMaxLength(100);
            entity.Property(e => e.TaxStatus).HasMaxLength(50);
            entity.Property(e => e.TaxNo).HasMaxLength(50);
            entity.Property(e => e.SSBStatus).HasMaxLength(50);
            entity.Property(e => e.SSCBNo).HasMaxLength(50);

            entity.Property(e => e.Currency).HasMaxLength(10);
            entity.Property(e => e.PayType).HasMaxLength(50);

            entity.Property(e => e.DateOfSalaryChanged).HasColumnType("date");
            entity.Property(e => e.DateOfCurrencyChange).HasColumnType("date");

            entity.Property(e => e.CreatedAt).HasColumnType("datetimeoffset").IsRequired();
            entity.Property(e => e.UpdatedAt).HasColumnType("datetimeoffset").IsRequired();
            entity.Property(e => e.Version).IsRowVersion();
        }
    }
}
