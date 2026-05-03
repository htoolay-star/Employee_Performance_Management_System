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
    public class PositionConfiguration : IEntityTypeConfiguration<Position>
    {
        public void Configure(EntityTypeBuilder<Position> entity)
        {
            
            entity.ToTable("Positions", "hr");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).UseIdentityColumn();

            entity.HasQueryFilter(e => !e.IsDeleted);

            entity.Property(e => e.PublicId).IsRequired();
            entity.HasIndex(e => e.PublicId).IsUnique();

            entity.HasIndex(e => e.Title).IsUnique();
            entity.Property(e => e.Title).HasMaxLength(100).IsRequired();

            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.Property(e => e.CreatedAt);
            entity.Property(e => e.UpdatedAt);

            entity.Property(e => e.Version).IsRowVersion();

            entity.HasOne(e => e.Level)
                  .WithMany()
                  .HasForeignKey(e => e.LevelId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.Metadata.FindNavigation(nameof(Position.PositionKPIs))?
                  .SetPropertyAccessMode(PropertyAccessMode.Field);

            entity.Metadata.FindNavigation(nameof(Position.PositionPermissions))?
                  .SetPropertyAccessMode(PropertyAccessMode.Field);

            entity.Metadata.FindNavigation(nameof(Position.PositionFormTemplates))?
                  .SetPropertyAccessMode(PropertyAccessMode.Field);

            entity.Metadata.FindNavigation(nameof(Position.PositionPIPTemplates))?
                  .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
