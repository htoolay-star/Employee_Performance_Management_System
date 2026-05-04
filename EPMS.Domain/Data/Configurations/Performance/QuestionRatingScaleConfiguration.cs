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
            builder.Property(e => e.MinScore).HasColumnType("decimal(5,2)").IsRequired();
            builder.Property(e => e.MaxScore).HasColumnType("decimal(5,2)").IsRequired();

            builder.Property(e => e.IsActive).HasDefaultValue(true);

            builder.Property(e => e.CreatedAt).IsRequired();
            builder.Property(e => e.UpdatedAt).IsRequired();

            builder.Property(e => e.Version).IsRowVersion();

            builder.Property(e => e.IsDeleted).HasDefaultValue(false).IsRequired();
            builder.Property(e => e.DeletedAt);
        }
    }
}
