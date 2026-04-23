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

            entity.Property(e => e.DateOfBirth);
            entity.Property(e => e.PassportExpireDate);

            entity.Property(e => e.WorkPermitNo).HasMaxLength(50);
            entity.Property(e => e.WorkPermitValidDate);
            entity.Property(e => e.WorkPermitExpireDate);
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.UpdatedAt).IsRequired();

            entity.Property(e => e.Version).IsRowVersion();

            entity.Property(e => e.ProfilePictureUrl).HasMaxLength(500);
            entity.Property(e => e.ProfileThumbnailUrl).HasMaxLength(500);

            entity.Property(e => e.AdditionalData);
        }
    }
}
