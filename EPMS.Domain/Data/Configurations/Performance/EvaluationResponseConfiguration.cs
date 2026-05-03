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
            builder.Property(e => e.PublicId).IsRequired();
            builder.HasIndex(e => e.PublicId).IsUnique().HasFilter("[IsDeleted] = 0");

            builder.HasIndex(e => new { e.AppraisalId, e.QuestionId, e.EvaluatorId }).IsUnique().HasFilter("[IsDeleted] = 0");

            builder.Property(e => e.EvaluatorRole).HasMaxLength(50).IsRequired();
            builder.Property(e => e.IsAnonymous).HasDefaultValue(false);
            builder.Property(e => e.YesNoAnswer);
            builder.Property(e => e.RatingValue);
            builder.Property(e => e.QuestionComment).HasMaxLength(500);

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

            builder.Property(e => e.IsDeleted).HasDefaultValue(false).IsRequired();
            builder.Property(e => e.DeletedAt);
        }
    }
}
