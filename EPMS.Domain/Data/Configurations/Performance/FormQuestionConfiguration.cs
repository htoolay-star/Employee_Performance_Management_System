using EPMS.Domain.Entities.Performance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPMS.Domain.Data.Configurations.Performance
{
    public class FormQuestionConfiguration : IEntityTypeConfiguration<FormQuestion>
    {
        public void Configure(EntityTypeBuilder<FormQuestion> builder)
        {
            builder.ToTable("FormQuestions", "perf");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).UseIdentityColumn();
            builder.Property(e => e.PublicId).IsRequired();
            builder.HasIndex(e => e.PublicId).IsUnique().HasFilter("[IsDeleted] = 0");

            builder.HasIndex(e => new { e.TemplateId, e.Sequence }).IsUnique().HasFilter("[IsDeleted] = 0");

            builder.Property(e => e.QuestionText).IsRequired();
            builder.Property(e => e.Sequence).IsRequired();

            builder.Property(e => e.QuestionRatingScaleId).IsRequired();

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


        }
    }
}
