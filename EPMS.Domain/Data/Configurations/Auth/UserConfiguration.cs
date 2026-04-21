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
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> entity)
        {
            entity.ToTable("Users", "auth");

            entity.HasKey(e => e.Id);

            entity.HasIndex(e => e.UserGuid).IsUnique();
            entity.Property(e => e.UserGuid)
                  .HasDefaultValueSql("NEWSEQUENTIALID()")
                  .ValueGeneratedOnAdd();

            entity.HasIndex(e => e.NormalizedEmail).IsUnique();
            entity.Property(e => e.Email).HasMaxLength(256).IsRequired();
            entity.Property(e => e.NormalizedEmail)
                  .HasMaxLength(256)
                  .HasComputedColumnSql("upper([Email])", stored: true)
                  .ValueGeneratedOnAddOrUpdate();

            entity.Property(e => e.PasswordHash).IsRequired();

            entity.Property(e => e.SecurityStamp)
                  .HasMaxLength(36)
                  .IsRequired();

            entity.Property(e => e.LockoutEndDate).HasColumnType("datetimeoffset");
            entity.Property(e => e.LastLoginDate).HasColumnType("datetimeoffset");

            entity.Property(e => e.CreatedAt).HasColumnType("datetimeoffset").HasDefaultValueSql("SYSUTCDATETIME()");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetimeoffset").HasDefaultValueSql("SYSUTCDATETIME()");

            entity.Property(e => e.Version).IsRowVersion();
        }
    }
}
