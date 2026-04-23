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
    public class FormQuestionConfiguration : IEntityTypeConfiguration<FormQuestion>
    {
        public void Configure(EntityTypeBuilder<FormQuestion> builder)
        {
            builder.ToTable("FormQuestions", "perf");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).UseIdentityColumn();

            builder.HasIndex(e => new { e.TemplateId, e.Sequence }).IsUnique();

            builder.Property(e => e.QuestionText).IsRequired();
            builder.Property(e => e.Sequence).IsRequired();

            builder.Property(e => e.HasYesNo).HasDefaultValue(false);
            builder.Property(e => e.HasComment).HasDefaultValue(false);

            builder.HasOne(e => e.Category)
                   .WithMany()
                   .HasForeignKey(e => e.CategoryId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.RatingScale)
                   .WithMany()
                   .HasForeignKey(e => e.QuestionRatingScaleId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(e => e.CreatedAt).HasColumnType("datetimeoffset").IsRequired();
            builder.Property(e => e.UpdatedAt).HasColumnType("datetimeoffset").IsRequired();
            builder.Property(e => e.Version).IsRowVersion();
        }
    }
}
