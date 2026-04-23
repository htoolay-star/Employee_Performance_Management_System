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
    public class EmployeeContactConfiguration : IEntityTypeConfiguration<EmployeeContact>
    {
        public void Configure(EntityTypeBuilder<EmployeeContact> entity)
        {
            entity.ToTable("EmployeeContacts", "hr");

            entity.HasKey(e => e.EmployeeId);

            entity.HasOne(e => e.Profile)
                  .WithOne(p => p.Contact)
                  .HasForeignKey<EmployeeContact>(e => e.EmployeeId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.ContactAddress).HasMaxLength(500);
            entity.Property(e => e.PermanentAddress).HasMaxLength(500);
            entity.Property(e => e.PhoneNo).HasMaxLength(50);
            entity.Property(e => e.PermanentPhoneNo).HasMaxLength(50);
            entity.Property(e => e.PresentPhoneNo).HasMaxLength(50);
            entity.Property(e => e.EmailAddress).HasMaxLength(100);
            entity.Property(e => e.InternalPhoneNo).HasMaxLength(50);
            entity.Property(e => e.EmergencyMobileNo).HasMaxLength(50);
            entity.Property(e => e.RelationWithEmergencyContact).HasMaxLength(50);

            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.UpdatedAt).IsRequired();
            entity.Property(e => e.Version).IsRowVersion();
        }
    }
}
