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
    public class ContinuousFeedbackConfiguration : IEntityTypeConfiguration<ContinuousFeedback>
    {
        public void Configure(EntityTypeBuilder<ContinuousFeedback> builder)
        {
            builder.ToTable("ContinuousFeedbacks", "perf");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).UseIdentityColumn();

            builder.Property(e => e.FeedbackType).HasMaxLength(50).IsRequired();
            builder.Property(e => e.Content).IsRequired();
            builder.Property(e => e.Visibility).HasMaxLength(20).HasDefaultValue("Public");

            builder.HasOne(e => e.Employee)
                   .WithMany()
                   .HasForeignKey(e => e.EmployeeId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.GivenBy)
                   .WithMany()
                   .HasForeignKey(e => e.GivenById)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.RelatedGoal)
                   .WithMany()
                   .HasForeignKey(e => e.RelatedGoalId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(e => e.CreatedAt).IsRequired();
            builder.Property(e => e.UpdatedAt).IsRequired();
            builder.Property(e => e.Version).IsRowVersion();
        }
    }
}
