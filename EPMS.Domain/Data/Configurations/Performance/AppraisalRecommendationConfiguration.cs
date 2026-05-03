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
    public class AppraisalRecommendationConfiguration : IEntityTypeConfiguration<AppraisalRecommendation>
    {
        public void Configure(EntityTypeBuilder<AppraisalRecommendation> builder)
        {
            builder.ToTable("AppraisalRecommendations", "perf");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).UseIdentityColumn();

            builder.HasQueryFilter(e => !e.IsDeleted);

            builder.Property(e => e.PublicId).IsRequired();
            builder.HasIndex(e => e.PublicId).IsUnique();

            builder.Property(e => e.RecommendationType).HasMaxLength(50).IsRequired();
            builder.Property(e => e.ProposedValue).HasMaxLength(200);
            builder.Property(e => e.Reason).IsRequired();
            builder.Property(e => e.Priority).HasMaxLength(20).HasDefaultValue("Normal");

            builder.Property(e => e.Status).HasMaxLength(20).HasDefaultValue("Pending");

            builder.HasOne(e => e.Appraisal)
                   .WithMany(a => a.Recommendations)
                   .HasForeignKey(e => e.AppraisalId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.ProcessedBy)
                   .WithMany()
                   .HasForeignKey(e => e.ProcessedById)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(e => e.CreatedAt).IsRequired();
            builder.Property(e => e.UpdatedAt).IsRequired();
            builder.Property(e => e.Version).IsRowVersion();
        }
    }
}
