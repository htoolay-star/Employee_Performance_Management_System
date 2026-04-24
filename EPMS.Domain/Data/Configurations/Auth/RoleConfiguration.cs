using EPMS.Domain.Entities.Auth;
using EPMS.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EPMS.Shared.Constants.AuthConstants;

namespace EPMS.Domain.Data.Configurations.Auth
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> entity)
        {
            entity.ToTable("Roles", "auth");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.Property(e => e.Name).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(250);

            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.UpdatedAt).IsRequired();

            entity.HasData(
                new { Id = AppRoles.Ids.SystemAdmin, Name = AppRoles.SystemAdmin, Description = "Full system access and security management.", CreatedAt = DateTimeOffset.UtcNow, UpdatedAt = DateTimeOffset.UtcNow },
                new { Id = AppRoles.Ids.Admin, Name = AppRoles.Admin, Description = "HR management and performance administration.", CreatedAt = DateTimeOffset.UtcNow, UpdatedAt = DateTimeOffset.UtcNow },
                new { Id = AppRoles.Ids.User, Name = AppRoles.User, Description = "Standard employee access for self-evaluations.", CreatedAt = DateTimeOffset.UtcNow, UpdatedAt = DateTimeOffset.UtcNow }
            );
        }
    }
}
