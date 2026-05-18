using EPMS.Domain.Entities.Performance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPMS.Domain.Data.Configurations.Performance
{
    public class QuestionRatingScaleLevelConfiguration : IEntityTypeConfiguration<QuestionRatingScaleLevel>
    {
        public void Configure(EntityTypeBuilder<QuestionRatingScaleLevel> builder)
        {
            builder.ToTable("QuestionRatingScaleLevels", "perf");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).UseIdentityColumn();
            builder.Property(e => e.PublicId).IsRequired();
            builder.HasIndex(e => e.PublicId).IsUnique().HasFilter("[IsDeleted] = 0");

            builder.HasIndex(e => new { e.QuestionRatingScaleId, e.Rating })
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");

            builder.Property(e => e.Rating).IsRequired();
            builder.Property(e => e.MinScore).HasColumnType("decimal(5,2)").IsRequired();
            builder.Property(e => e.MaxScore).HasColumnType("decimal(5,2)").IsRequired();

            builder.Property(e => e.CreatedAt).IsRequired();
            builder.Property(e => e.UpdatedAt).IsRequired();
            builder.Property(e => e.Version).IsRowVersion();

            builder.Property(e => e.IsDeleted).HasDefaultValue(false).IsRequired();
            builder.Property(e => e.DeletedAt);

            builder.HasOne(e => e.QuestionRatingScale)
                .WithMany(s => s.Levels)
                .HasForeignKey(e => e.QuestionRatingScaleId)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
