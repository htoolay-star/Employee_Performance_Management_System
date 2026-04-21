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
    public class EmployeeEmploymentConfiguration : IEntityTypeConfiguration<EmployeeEmployment>
    {
        public void Configure(EntityTypeBuilder<EmployeeEmployment> entity)
        {
            entity.ToTable("EmployeeEmployment", "hr");

            entity.HasKey(e => e.EmployeeId);

            entity.HasOne(e => e.Profile)
                  .WithOne(p => p.Employment)
                  .HasForeignKey<EmployeeEmployment>(e => e.EmployeeId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.DirectManager)
                  .WithMany()
                  .HasForeignKey(e => e.DirectManagerId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Department).WithMany().HasForeignKey(e => e.DepartmentId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.ParentDepartment).WithMany().HasForeignKey(e => e.ParentDepartmentId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Team).WithMany().HasForeignKey(e => e.TeamId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Position).WithMany().HasForeignKey(e => e.PositionId).OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.EmploymentStatus).HasMaxLength(50).IsRequired();
            entity.Property(e => e.StaffType).HasMaxLength(50);
            entity.Property(e => e.Shift).HasMaxLength(50);
            entity.Property(e => e.FingerPrintId).HasMaxLength(50);

            entity.Property(e => e.DateOfAppointment).HasColumnType("date");
            entity.Property(e => e.DateOfConfirmation).HasColumnType("date");
            entity.Property(e => e.DateOfPromotion).HasColumnType("date");
            entity.Property(e => e.DateOfTermination).HasColumnType("date");
            entity.Property(e => e.DateOfTransfer).HasColumnType("date");
            entity.Property(e => e.DateOfDemotion).HasColumnType("date");
            entity.Property(e => e.DateOfTitleChange).HasColumnType("date");

            entity.Property(e => e.DateOfIncrement).HasColumnType("date");
            entity.Property(e => e.ProductProject).HasMaxLength(200);

            entity.Property(e => e.CreatedAt).HasColumnType("datetimeoffset").IsRequired();
            entity.Property(e => e.UpdatedAt).HasColumnType("datetimeoffset").IsRequired();
            entity.Property(e => e.Version).IsRowVersion();
        }
    }
}
