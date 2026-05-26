using EPMS.Domain.Entities.Performance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPMS.Domain.Data.Configurations.Performance;

public class AppraisalConfiguration : IEntityTypeConfiguration<Appraisal>
{
    public void Configure(EntityTypeBuilder<Appraisal> entity)
    {
        entity.ToTable("Appraisals", "perf");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityColumn();

        entity.Property(e => e.PublicId).IsRequired();
        entity.HasIndex(e => e.PublicId).IsUnique().HasFilter("[IsDeleted] = 0");

        entity.HasIndex(e => new { e.EmployeeId, e.CycleId })
            .IsUnique().HasFilter("[IsDeleted] = 0 AND [EmployeeId] IS NOT NULL");

        entity.HasIndex(e => new { e.EntityType, e.EntityId, e.CycleId })
            .IsUnique().HasFilter("[IsDeleted] = 0 AND [EntityType] IS NOT NULL");

        entity.Property(e => e.EntityType).HasMaxLength(20);
        entity.Property(e => e.Status)
              .HasMaxLength(20)
              .IsRequired();

        entity.Property(e => e.KpiStatus)
              .HasMaxLength(20)
              .IsRequired()
              .HasDefaultValue("DRAFT");

        entity.Property(e => e.SelfStatus)
              .HasMaxLength(20)
              .IsRequired()
              .HasDefaultValue("DRAFT");

        entity.Property(e => e.ManagerStatus)
              .HasMaxLength(20)
              .IsRequired()
              .HasDefaultValue("DRAFT");

        entity.Property(e => e.PeerStatus)
              .HasMaxLength(20)
              .IsRequired()
              .HasDefaultValue("DRAFT");

        entity.Property(e => e.SubordinateStatus)
              .HasMaxLength(20)
              .IsRequired()
              .HasDefaultValue("DRAFT");

        entity.Property(e => e.CommitteeStatus)
              .HasMaxLength(20)
              .IsRequired()
              .HasDefaultValue("DRAFT");

        entity.Property(e => e.TotalScore)
              .HasPrecision(5, 2);

        entity.Property(e => e.KpiScore)
              .HasPrecision(5, 2);

        entity.Property(e => e.SelfScore)
              .HasPrecision(5, 2);

        entity.Property(e => e.ThreeSixtyScore)
              .HasPrecision(5, 2);

        entity.Property(e => e.AppraisalScore)
              .HasPrecision(5, 2);

        entity.Property(e => e.FormulaWeights)
              .HasMaxLength(100);

        entity.Property(e => e.RatingLabel).HasMaxLength(50);
        entity.Property(e => e.EmployeeComment).HasMaxLength(500);
        entity.Property(e => e.ManagerComment).HasMaxLength(500);
        entity.Property(e => e.ReviewDate);
        entity.Property(e => e.UnLockReason).HasMaxLength(500);
        entity.Property(e => e.IsLocked).HasDefaultValue(false);
        entity.Property(e => e.LockedAt);
        entity.Property(e => e.FinalizedDate);
        entity.Property(e => e.UnLockedAt);

        entity.Property(e => e.SelfLocked).HasDefaultValue(false);
        entity.Property(e => e.SelfLockIsDeadline).HasDefaultValue(false);
        entity.Property(e => e.KpiLocked).HasDefaultValue(false);
        entity.Property(e => e.KpiLockIsDeadline).HasDefaultValue(false);
        entity.Property(e => e.ThreeSixtyLocked).HasDefaultValue(false);
        entity.Property(e => e.ThreeSixtyLockIsDeadline).HasDefaultValue(false);
        entity.Property(e => e.AppraisalLocked).HasDefaultValue(false);
        entity.Property(e => e.AppraisalLockIsDeadline).HasDefaultValue(false);

        entity.Property(e => e.CreatedAt).IsRequired();
        entity.Property(e => e.UpdatedAt).IsRequired();
        entity.Property(e => e.Version).IsRowVersion();

        entity.Property(e => e.IsDeleted).HasDefaultValue(false).IsRequired();
        entity.Property(e => e.DeletedAt);

        entity.HasOne(e => e.Employee).WithMany().HasForeignKey(e => e.EmployeeId).OnDelete(DeleteBehavior.Restrict);
        entity.HasOne(e => e.Cycle).WithMany().HasForeignKey(e => e.CycleId).OnDelete(DeleteBehavior.Restrict);
        entity.HasOne(e => e.ManagerReviewer).WithMany().HasForeignKey(e => e.ManagerReviewerId).OnDelete(DeleteBehavior.Restrict);

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

        entity.Metadata.FindNavigation(nameof(Appraisal.Responses))?
              .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
