using EPMS.Domain.Entities.Performance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPMS.Domain.Data.Configurations.Performance
{
    public class QuestionRatingScaleConfiguration : IEntityTypeConfiguration<QuestionRatingScale>
    {
        public void Configure(EntityTypeBuilder<QuestionRatingScale> builder)
        {
            builder.ToTable("QuestionRatingScales", "perf");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).UseIdentityColumn();
            builder.Property(e => e.PublicId).IsRequired();
            builder.HasIndex(e => e.PublicId).IsUnique().HasFilter("[IsDeleted] = 0");

            builder.HasIndex(e => e.Name).IsUnique().HasFilter("[IsDeleted] = 0");

            builder.Property(e => e.Name).HasMaxLength(100).IsRequired();

            builder.Property(e => e.IsActive).HasDefaultValue(true);

            builder.Property(e => e.CreatedAt).IsRequired();
            builder.Property(e => e.UpdatedAt).IsRequired();

            builder.Property(e => e.Version).IsRowVersion();

            builder.Property(e => e.IsDeleted).HasDefaultValue(false).IsRequired();
            builder.Property(e => e.DeletedAt);

            builder.HasMany(e => e.Levels)
                .WithOne(l => l.QuestionRatingScale)
                .HasForeignKey(l => l.QuestionRatingScaleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
