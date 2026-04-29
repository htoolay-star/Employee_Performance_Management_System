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

            // 1. Business Rules & Constraints
            entity.HasIndex(e => new { e.EmployeeId, e.CycleId, e.EvaluatorRole }).IsUnique();

            // 2. Enums (Chained with String Conversion and MaxLength)
            entity.Property(e => e.Status)
                  .HasConversion<string>()
                  .HasMaxLength(20)
                  .IsRequired();

            entity.Property(e => e.EvaluatorRole)
                  .HasConversion<string>()
                  .HasMaxLength(50)
                  .IsRequired();

            // 3. Properties
            entity.Property(e => e.TotalScore)
                  .HasPrecision(5, 2);

            entity.Property(e => e.RatingLabel).HasMaxLength(50);
            entity.Property(e => e.UnLockReason).HasMaxLength(500);
            entity.Property(e => e.IsLocked).HasDefaultValue(false);

            // 4. Audit & Soft Delete
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.UpdatedAt).IsRequired();
            entity.Property(e => e.Version).IsRowVersion();

            // 5. Navigation Properties (Foreign Keys)
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

            // 6. Private Collection Backing Fields (CRITICAL FOR DDD)
            entity.Metadata.FindNavigation(nameof(Appraisal.Details))?
                  .SetPropertyAccessMode(PropertyAccessMode.Field);

            entity.Metadata.FindNavigation(nameof(Appraisal.Recommendations))? // Added this one!
                  .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}