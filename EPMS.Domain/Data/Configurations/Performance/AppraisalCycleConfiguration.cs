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

            entity.HasIndex(e => new { e.Name, e.Year }).IsUnique().HasFilter("[IsDeleted] = 0");

            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
            entity.Property(e => e.AppraisalType).HasMaxLength(20).IsRequired();

            entity.Property(e => e.StartDate).IsRequired();
            entity.Property(e => e.EndDate).IsRequired();

            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsLocked).HasDefaultValue(false);

            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.UpdatedAt).IsRequired();
            entity.Property(e => e.Version).IsRowVersion();

            entity.Property(e => e.IsDeleted).HasDefaultValue(false).IsRequired();
            entity.Property(e => e.DeletedAt);

            entity.Property(e => e.PeerReviewStartDate);
            entity.Property(e => e.PeerReviewDeadline);
            entity.Property(e => e.SelfReviewStartDate);
            entity.Property(e => e.SelfReviewDeadline);
            entity.Property(e => e.ManagerReviewStartDate);
            entity.Property(e => e.ManagerReviewDeadline);

            entity.Property(e => e.FinalClosureDate);
        }
    }
}
