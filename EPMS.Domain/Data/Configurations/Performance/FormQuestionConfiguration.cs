using EPMS.Domain.Entities.Performance;
using EPMS.Domain.Entities.Shared;
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

            builder.HasQueryFilter(e => !e.IsDeleted);

            builder.Property(e => e.PublicId).IsRequired();
            builder.HasIndex(e => e.PublicId).IsUnique().HasFilter("[IsDeleted] = 0");

            builder.HasIndex(e => new { e.TemplateId, e.Sequence }).IsUnique().HasFilter("[IsDeleted] = 0");

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

            builder.Property(e => e.CreatedAt).IsRequired();
            builder.Property(e => e.UpdatedAt).IsRequired();
            builder.Property(e => e.Version).IsRowVersion();

            builder.Property(e => e.IsDeleted).HasDefaultValue(false).IsRequired();
            builder.Property(e => e.DeletedAt);

            builder.HasMany(e => e.Tags)
                   .WithMany()
                   .UsingEntity<Dictionary<string, object>>(
                       "FormQuestionTag",

                       right => right.HasOne<Tag>()
                                     .WithMany()
                                     .HasForeignKey("TagId")
                                     .OnDelete(DeleteBehavior.Cascade),

                       left => left.HasOne<FormQuestion>()
                                   .WithMany()
                                   .HasForeignKey("QuestionId")
                                   .OnDelete(DeleteBehavior.Cascade),

                       join =>
                       {
                           join.ToTable("FormQuestionTags", "perf");
                           join.HasKey("QuestionId", "TagId");
                       }
                   );

            builder.Metadata.FindNavigation(nameof(FormQuestion.Tags))?.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
