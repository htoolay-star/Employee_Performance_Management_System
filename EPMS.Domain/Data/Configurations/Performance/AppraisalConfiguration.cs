using EPMS.Domain.Entities.Performance; // Assuming your namespace
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPMS.Domain.Data.Configurations.Performance
{
    public class AppraisalConfiguration : IEntityTypeConfiguration<Appraisal>
    {
        public void Configure(EntityTypeBuilder<Appraisal> entity)
        {
            entity.ToTable("Appraisals", "perf");
            entity.HasKey(e => e.Id);

            entity.HasQueryFilter(e => !e.IsDeleted);

            entity.Property(e => e.PublicId).IsRequired();
            entity.HasIndex(e => e.PublicId).IsUnique().HasFilter("[IsDeleted] = 0");

            entity.HasIndex(e => new { e.EmployeeId, e.CycleId, e.EvaluatorRole }).IsUnique().HasFilter("[IsDeleted] = 0");

            entity.Property(e => e.Status)
                  .HasConversion<string>()
                  .HasMaxLength(20)
                  .IsRequired();

            entity.Property(e => e.EvaluatorRole)
                  .HasConversion<string>()
                  .HasMaxLength(50)
                  .IsRequired();

            entity.Property(e => e.TotalScore)
                  .HasPrecision(5, 2);

            entity.Property(e => e.RatingLabel).HasMaxLength(50);
            entity.Property(e => e.EmployeeComment).HasMaxLength(500);
            entity.Property(e => e.ManagerComment).HasMaxLength(500);
            entity.Property(e => e.ReviewDate);
            entity.Property(e => e.UnLockReason).HasMaxLength(500);
            entity.Property(e => e.IsLocked).HasDefaultValue(false);
            entity.Property(e => e.LockedAt);
            entity.Property(e => e.FinalizedDate);
            entity.Property(e => e.UnLockedAt);

            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.UpdatedAt).IsRequired();
            entity.Property(e => e.Version).IsRowVersion();

            entity.Property(e => e.IsDeleted).HasDefaultValue(false).IsRequired();
            entity.Property(e => e.DeletedAt);

            entity.HasOne(e => e.Employee).WithMany().HasForeignKey(e => e.EmployeeId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Cycle).WithMany().HasForeignKey(e => e.CycleId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Appraiser).WithMany().HasForeignKey(e => e.AppraiserId).OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.FinalRating)
                  .WithMany()
                  .HasForeignKey(e => e.FinalRatingId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.UnLockedBy)
                  .WithMany()
                  .HasForeignKey(e => e.UnLockedById)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.Metadata.FindNavigation(nameof(Appraisal.Details))?
                  .SetPropertyAccessMode(PropertyAccessMode.Field);

            entity.Metadata.FindNavigation(nameof(Appraisal.Recommendations))?
                  .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
