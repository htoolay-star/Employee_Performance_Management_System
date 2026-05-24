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
    public class AppraisalCycleConfiguration : IEntityTypeConfiguration<AppraisalCycle>
    {
        public void Configure(EntityTypeBuilder<AppraisalCycle> entity)
        {
            entity.ToTable("AppraisalCycles", "perf");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).UseIdentityColumn();
            entity.Property(e => e.PublicId).IsRequired();
            entity.HasIndex(e => e.PublicId).IsUnique().HasFilter("[IsDeleted] = 0");

            entity.HasIndex(e => new { e.Name, e.YearLabel }).IsUnique().HasFilter("[IsDeleted] = 0");

            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
            entity.Property(e => e.AppraisalType).HasMaxLength(20).IsRequired();
            entity.Property(e => e.CalendarType).HasMaxLength(50).IsRequired();
            entity.Property(e => e.YearLabel).HasMaxLength(50).IsRequired();

            entity.Property(e => e.EvaluationStartDate).IsRequired();
            entity.Property(e => e.EvaluationEndDate).IsRequired();
            entity.Property(e => e.WindowStartDate).IsRequired();
            entity.Property(e => e.WindowEndDate).IsRequired();

            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsLocked).HasDefaultValue(false);

            entity.Property(e => e.KpiWeight).HasPrecision(5, 2).HasDefaultValue(50m);
            entity.Property(e => e.SelfWeight).HasPrecision(5, 2).HasDefaultValue(15m);
            entity.Property(e => e.ThreeSixtyWeight).HasPrecision(5, 2).HasDefaultValue(10m);
            entity.Property(e => e.AppraisalWeight).HasPrecision(5, 2).HasDefaultValue(25m);

            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.UpdatedAt).IsRequired();
            entity.Property(e => e.Version).IsRowVersion();

            entity.Property(e => e.IsDeleted).HasDefaultValue(false).IsRequired();
            entity.Property(e => e.DeletedAt);

            entity.Property(e => e.ThreeSixtyReviewStartDate);
            entity.Property(e => e.ThreeSixtyReviewDeadline);
            entity.Property(e => e.SelfReviewStartDate);
            entity.Property(e => e.SelfReviewDeadline);
            entity.Property(e => e.ManagerReviewStartDate);
            entity.Property(e => e.ManagerReviewDeadline);

            entity.Property(e => e.FinalClosureDate);

            entity.HasMany(e => e.EmployeeKPIs)
                  .WithOne(e => e.Cycle)
                  .HasForeignKey(e => e.CycleId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
