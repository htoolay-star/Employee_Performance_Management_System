using EPMS.Domain.Entities.Hr;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPMS.Domain.Data.Configurations.Hr
{
    public class PositionConfiguration : IEntityTypeConfiguration<Position>
    {
        public void Configure(EntityTypeBuilder<Position> entity)
        {

            entity.ToTable("Positions", "hr");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).UseIdentityColumn();
            entity.Property(e => e.PublicId).IsRequired();
            entity.HasIndex(e => e.PublicId).IsUnique().HasFilter("[IsDeleted] = 0");

            entity.HasIndex(e => e.Code).IsUnique().HasFilter("[IsDeleted] = 0");
            entity.Property(e => e.Code).HasMaxLength(20).IsRequired();

            entity.HasIndex(e => e.Name).IsUnique().HasFilter("[IsDeleted] = 0");
            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();

            entity.Property(e => e.Description).HasMaxLength(500);

            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.HasIndex(e => e.IsActive).HasFilter("[IsActive] = 1");

            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.UpdatedAt).IsRequired();

            entity.Property(e => e.Version).IsRowVersion();

            entity.Property(e => e.IsDeleted).HasDefaultValue(false).IsRequired();
            entity.Property(e => e.DeletedAt);

            entity.HasOne(e => e.Level)
                  .WithMany()
                  .HasForeignKey(e => e.LevelId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.Metadata.FindNavigation(nameof(Position.PositionPermissions))?
                  .SetPropertyAccessMode(PropertyAccessMode.Field);

            entity.Metadata.FindNavigation(nameof(Position.PositionFormTemplates))?
                  .SetPropertyAccessMode(PropertyAccessMode.Field);

            entity.Metadata.FindNavigation(nameof(Position.PositionPIPTemplates))?
                  .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
