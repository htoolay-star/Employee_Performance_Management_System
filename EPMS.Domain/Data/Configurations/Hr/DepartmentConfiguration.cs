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
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> entity)
        {
            entity.ToTable("Departments", "hr");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).UseIdentityColumn();

            entity.HasQueryFilter(e => !e.IsDeleted);

            entity.Property(e => e.PublicId).IsRequired();
            entity.HasIndex(e => e.PublicId).IsUnique();

            entity.HasIndex(e => e.Code).IsUnique();
            entity.Property(e => e.Code).HasMaxLength(20).IsRequired();

            entity.HasIndex(e => e.Name).IsUnique();
            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();

            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.UpdatedAt).IsRequired();

            entity.Property(e => e.Version).IsRowVersion();

            entity.HasMany(e => e.Teams)
                  .WithOne(t => t.Department)
                  .HasForeignKey(t => t.DepartmentId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.Metadata.FindNavigation(nameof(Department.Teams))?
                  .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
