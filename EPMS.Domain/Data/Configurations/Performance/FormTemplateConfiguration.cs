using EPMS.Domain.Entities.Performance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPMS.Domain.Data.Configurations.Performance
{
    public class FormTemplateConfiguration : IEntityTypeConfiguration<FormTemplate>
    {
        public void Configure(EntityTypeBuilder<FormTemplate> builder)
        {
            builder.ToTable("FormTemplates", "perf");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).UseIdentityColumn();
            builder.Property(e => e.PublicId).IsRequired();
            builder.HasIndex(e => e.PublicId).IsUnique().HasFilter("[IsDeleted] = 0");

            builder.HasIndex(e => e.Name).IsUnique().HasFilter("[IsDeleted] = 0");

            builder.Property(e => e.Name).HasMaxLength(100).IsRequired();
            builder.Property(e => e.FormType).HasMaxLength(50).IsRequired();
            builder.Property(e => e.QuestionRatingScaleId).IsRequired();
            builder.Property(e => e.QuestionsPerEvaluation);
            builder.Property(e => e.IsActive).HasDefaultValue(true);
            builder.Property(e => e.HasYesNo).HasDefaultValue(false);
            builder.Property(e => e.HasComment).HasDefaultValue(false);

            builder.Property(e => e.CreatedAt).IsRequired();
            builder.Property(e => e.UpdatedAt).IsRequired();
            builder.Property(e => e.Version).IsRowVersion();

            builder.Property(e => e.IsDeleted).HasDefaultValue(false).IsRequired();
            builder.Property(e => e.DeletedAt);

            builder.HasOne(e => e.RatingScale)
                   .WithMany()
                   .HasForeignKey(e => e.QuestionRatingScaleId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.Questions)
                   .WithOne(q => q.Template)
                   .HasForeignKey(q => q.TemplateId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Metadata.FindNavigation(nameof(FormTemplate.Questions))?
                   .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
