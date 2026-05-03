using EPMS.Domain.Entities.Hr;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Data.Configurations.Hr
{
    public class RatingScaleConfiguration : IEntityTypeConfiguration<RatingScale>
    {
        public void Configure(EntityTypeBuilder<RatingScale> entity)
        {
            entity.ToTable("RatingScales", "hr");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).UseIdentityColumn();

            entity.HasQueryFilter(e => !e.IsDeleted);

            entity.Property(e => e.PublicId).IsRequired();
            entity.HasIndex(e => e.PublicId).IsUnique();

            entity.HasIndex(e => e.Rating).IsUnique();

            entity.Property(e => e.Label).HasMaxLength(50).IsRequired();

            entity.Property(e => e.MinScore).HasColumnType("decimal(5,2)").IsRequired();
            entity.Property(e => e.MaxScore).HasColumnType("decimal(5,2)").IsRequired();

            entity.Property(e => e.PerformanceLevel).HasMaxLength(50);
            entity.Property(e => e.PromotionEligibility).HasMaxLength(100);

            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.UpdatedAt).IsRequired();
            entity.Property(e => e.Version).IsRowVersion();
        }
    }
}
