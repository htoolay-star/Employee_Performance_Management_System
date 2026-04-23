using EPMS.Domain.Entities.Performance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Data.Configurations.Performance
{
    public class OneOnOneMeetingConfiguration : IEntityTypeConfiguration<OneOnOneMeeting>
    {
        public void Configure(EntityTypeBuilder<OneOnOneMeeting> builder)
        {
            builder.ToTable("OneOnOneMeetings", "perf");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).UseIdentityColumn();

            builder.Property(e => e.Title).HasMaxLength(200).IsRequired();
            builder.Property(e => e.Summary).HasMaxLength(500);
            builder.Property(e => e.Status).HasMaxLength(20).HasDefaultValue("Scheduled");

            builder.Property(e => e.IsAcknowledgedByEmployee).HasDefaultValue(false);

            builder.HasOne(e => e.Employee)
                   .WithMany()
                   .HasForeignKey(e => e.EmployeeId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Manager)
                   .WithMany()
                   .HasForeignKey(e => e.ManagerId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(e => e.CreatedAt).IsRequired();
            builder.Property(e => e.UpdatedAt).IsRequired();
            builder.Property(e => e.Version).IsRowVersion();

            builder.Property(e => e.MeetingType).HasMaxLength(50).HasDefaultValue("Regular");

            builder.HasOne(e => e.RelatedPIP)
                   .WithMany()
                   .HasForeignKey(e => e.RelatedPIPId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
