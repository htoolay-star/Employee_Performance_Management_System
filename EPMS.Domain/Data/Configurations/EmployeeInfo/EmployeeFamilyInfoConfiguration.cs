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
    public class EmployeeFamilyInfoConfiguration : IEntityTypeConfiguration<EmployeeFamilyInfo>
    {
        public void Configure(EntityTypeBuilder<EmployeeFamilyInfo> entity)
        {
            entity.ToTable("EmployeeFamilyInfo", "hr");

            entity.HasKey(e => e.EmployeeId);

            entity.Ignore(e => e.Id);
            entity.Property(e => e.PublicId).IsRequired();
            entity.HasIndex(e => e.PublicId).IsUnique().HasFilter("[IsDeleted] = 0");

            entity.HasOne(e => e.Profile)
                  .WithOne(p => p.FamilyInfo)
                  .HasForeignKey<EmployeeFamilyInfo>(e => e.EmployeeId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.MaritalStatus).HasMaxLength(20);
            entity.Property(e => e.SpouseName).HasMaxLength(100);
            entity.Property(e => e.SpouseNRCNo).HasMaxLength(50);
            entity.Property(e => e.SpouseOccupation).HasMaxLength(100);

            entity.Property(e => e.FatherName).HasMaxLength(100);
            entity.Property(e => e.FatherNRCNo).HasMaxLength(50);
            entity.Property(e => e.FatherOccupation).HasMaxLength(100);

            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.UpdatedAt).IsRequired();
            entity.Property(e => e.Version).IsRowVersion();

            entity.Property(e => e.IsDeleted).HasDefaultValue(false).IsRequired();
            entity.Property(e => e.DeletedAt);
        }
    }
}
