using EPMS.Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Data.Configurations.Auth
{
    public class PositionRoleConfiguration : IEntityTypeConfiguration<PositionRole>
    {
        public void Configure(EntityTypeBuilder<PositionRole> entity)
        {
            entity.ToTable("PositionRoles", "auth");

            entity.HasKey(e => e.Id);

            // Composite index for unique constraint on PositionId + RoleId
            entity.HasIndex(e => new { e.PositionId, e.RoleId })
                  .IsUnique()
                  .HasFilter("[IsDeleted] = 0");

            // Foreign key to Position
            entity.HasOne(e => e.Position)
                   .WithMany(p => p.PositionRoles)
                   .HasForeignKey(e => e.PositionId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Foreign key to Role
            entity.HasOne(e => e.Role)
                   .WithMany()
                   .HasForeignKey(e => e.RoleId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Standard audit columns
            entity.Property(e => e.PublicId).IsRequired();
            entity.HasIndex(e => e.PublicId).IsUnique().HasFilter("[IsDeleted] = 0");

            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.UpdatedAt).IsRequired();

            // Soft delete properties
            entity.Property(e => e.IsDeleted).HasDefaultValue(false).IsRequired();
            entity.Property(e => e.DeletedAt);

            // Version property for optimistic concurrency
            entity.Property(e => e.Version).IsRowVersion();
        }
    }
}