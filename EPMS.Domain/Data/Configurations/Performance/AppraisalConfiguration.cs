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
    public class AppraisalConfiguration : IEntityTypeConfiguration<Appraisal>
    {
        public void Configure(EntityTypeBuilder<Appraisal> entity)
        {
            entity.ToTable("Appraisals", "perf");
            entity.HasKey(e => e.Id);

            entity.HasIndex(e => new { e.EmployeeId, e.CycleId }).IsUnique();

            entity.Property(e => e.Status).HasMaxLength(20).IsRequired();
            entity.Property(e => e.TotalScore);
            entity.Property(e => e.RatingLabel).HasMaxLength(50);

            entity.HasOne(e => e.Employee).WithMany().HasForeignKey(e => e.EmployeeId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Cycle).WithMany().HasForeignKey(e => e.CycleId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Appraiser).WithMany().HasForeignKey(e => e.AppraiserId).OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.FinalRating)
                  .WithMany()
                  .HasForeignKey(e => e.FinalRatingId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.Metadata.FindNavigation(nameof(Appraisal.Details))?
                  .SetPropertyAccessMode(PropertyAccessMode.Field);

            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.UpdatedAt).IsRequired();
            entity.Property(e => e.Version).IsRowVersion();

            entity.Property(e => e.IsLocked).HasDefaultValue(false);
            entity.Property(e => e.UnLockReason).HasMaxLength(500);

            entity.HasOne(e => e.UnLockedBy)
                   .WithMany()
                   .HasForeignKey(e => e.UnLockedById)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
