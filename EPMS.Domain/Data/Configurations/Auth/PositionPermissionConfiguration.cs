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
    public class PositionPermissionConfiguration : IEntityTypeConfiguration<PositionPermission>
    {
        public void Configure(EntityTypeBuilder<PositionPermission> entity)
        {
            entity.ToTable("PositionPermissions", "auth");

            entity.HasKey(e => new { e.PositionId, e.PermissionId });

            entity.Property(e => e.CreatedAt)
                  .HasDefaultValueSql("SYSDATETIMEOFFSET()")
                  .ValueGeneratedOnAdd();

            entity.HasOne(e => e.Permission)
                  .WithMany()
                  .HasForeignKey(e => e.PermissionId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Position)
                  .WithMany(p => p.PositionPermissions)
                  .HasForeignKey(e => e.PositionId)
                  .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
