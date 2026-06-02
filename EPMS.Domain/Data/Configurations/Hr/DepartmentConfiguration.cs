using EPMS.Domain.Entities.Hr;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPMS.Domain.Data.Configurations.Hr
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> entity)
        {
            entity.ToTable("Departments", "hr");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).UseIdentityColumn();
            entity.Property(e => e.PublicId).IsRequired();
            entity.HasIndex(e => e.PublicId).IsUnique().HasFilter("[IsDeleted] = 0");

            entity.HasIndex(e => e.Code).IsUnique().HasFilter("[IsDeleted] = 0");
            entity.Property(e => e.Code).HasMaxLength(20).IsRequired();

            entity.HasIndex(e => e.Name).IsUnique().HasFilter("[IsDeleted] = 0");
            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();

            entity.Property(e => e.Description).HasMaxLength(500);

            entity.HasIndex(e => e.DeptHeadId).IsUnique();
            entity.Property(e => e.DeptHeadId);
            entity.HasOne(e => e.DeptHead)
                  .WithMany()
                  .HasForeignKey(e => e.DeptHeadId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.HasIndex(e => e.IsActive).HasFilter("[IsActive] = 1");

            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.UpdatedAt).IsRequired();

            entity.Property(e => e.Version).IsRowVersion();

            entity.Property(e => e.IsDeleted).HasDefaultValue(false).IsRequired();
            entity.Property(e => e.DeletedAt);

            entity.HasMany(e => e.Teams)
                  .WithOne(t => t.Department)
                  .HasForeignKey(t => t.DepartmentId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.Metadata.FindNavigation(nameof(Department.Teams))?
                  .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
