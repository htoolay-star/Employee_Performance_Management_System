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
    public class EvaluationResponseConfiguration : IEntityTypeConfiguration<EvaluationResponse>
    {
        public void Configure(EntityTypeBuilder<EvaluationResponse> builder)
        {
            builder.ToTable("EvaluationResponses", "perf");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).UseIdentityColumn();

            builder.HasQueryFilter(e => !e.IsDeleted);

            builder.Property(e => e.PublicId).IsRequired();
            builder.HasIndex(e => e.PublicId).IsUnique();

            builder.HasIndex(e => new { e.AppraisalId, e.QuestionId, e.EvaluatorId }).IsUnique();

            builder.Property(e => e.EvaluatorRole).HasMaxLength(50).IsRequired();
            builder.Property(e => e.IsAnonymous).HasDefaultValue(false);

            builder.HasOne(e => e.Appraisal)
                   .WithMany()
                   .HasForeignKey(e => e.AppraisalId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Template)
                   .WithMany()
                   .HasForeignKey(e => e.TemplateId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Question)
                   .WithMany()
                   .HasForeignKey(e => e.QuestionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Evaluator)
                   .WithMany()
                   .HasForeignKey(e => e.EvaluatorId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(e => e.CreatedAt).IsRequired();
            builder.Property(e => e.UpdatedAt).IsRequired();
            builder.Property(e => e.Version).IsRowVersion();
        }
    }
}
