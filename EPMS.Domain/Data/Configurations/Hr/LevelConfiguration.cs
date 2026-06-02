using EPMS.Domain.Entities.Hr;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPMS.Domain.Data.Configurations.Hr
{
    public class LevelConfiguration : IEntityTypeConfiguration<Level>
    {
        public void Configure(EntityTypeBuilder<Level> entity)
        {
            entity.ToTable("Levels", "hr");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).UseIdentityColumn();
            entity.Property(e => e.PublicId).IsRequired();
            entity.HasIndex(e => e.PublicId).IsUnique().HasFilter("[IsDeleted] = 0");

            entity.HasIndex(e => e.Code).IsUnique().HasFilter("[IsDeleted] = 0");
            entity.Property(e => e.Code).HasMaxLength(10).IsRequired();

            entity.HasIndex(e => e.Name).IsUnique().HasFilter("[IsDeleted] = 0");
            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(250);

            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.HasIndex(e => e.IsActive).HasFilter("[IsActive] = 1");

            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.UpdatedAt).IsRequired();

            entity.Property(e => e.Version).IsRowVersion();

            entity.Property(e => e.IsDeleted).HasDefaultValue(false).IsRequired();
            entity.Property(e => e.DeletedAt);
        }
    }
}
