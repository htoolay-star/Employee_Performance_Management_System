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
    public class EmployeeProfileConfiguration : IEntityTypeConfiguration<EmployeeProfile>
    {
        public void Configure(EntityTypeBuilder<EmployeeProfile> entity)
        {
            entity.ToTable("EmployeeProfiles", "hr");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).UseIdentityColumn();

            entity.HasIndex(e => e.UserId).IsUnique();

            entity.HasIndex(e => e.StaffNo).IsUnique();
            entity.Property(e => e.StaffNo).HasMaxLength(50).IsRequired();

            entity.Property(e => e.FirstName).HasMaxLength(100).IsRequired();
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.OtherName).HasMaxLength(100);

            entity.Property(e => e.NRCNo).HasMaxLength(50);
            entity.Property(e => e.Gender).HasMaxLength(20);
            entity.Property(e => e.Race).HasMaxLength(50);
            entity.Property(e => e.Religion).HasMaxLength(50);
            entity.Property(e => e.Nationality).HasMaxLength(50);
            entity.Property(e => e.BirthPlace).HasMaxLength(250);
            entity.Property(e => e.PassportNo).HasMaxLength(50);
            entity.Property(e => e.LabourRegistrationNo).HasMaxLength(50);

            entity.Property(e => e.DateOfBirth).HasColumnType("date");
            entity.Property(e => e.PassportExpireDate).HasColumnType("date");

            entity.Property(e => e.WorkPermitNo).HasMaxLength(50);
            entity.Property(e => e.WorkPermitValidDate).HasColumnType("date");
            entity.Property(e => e.WorkPermitExpireDate).HasColumnType("date");

            entity.Property(e => e.CreatedAt).HasColumnType("datetimeoffset").IsRequired();
            entity.Property(e => e.UpdatedAt).HasColumnType("datetimeoffset").IsRequired();

            entity.Property(e => e.Version).IsRowVersion();
        }
    }
}
