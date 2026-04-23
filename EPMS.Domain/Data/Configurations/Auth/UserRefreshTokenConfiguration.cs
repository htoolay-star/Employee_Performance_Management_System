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
    public class UserRefreshTokenConfiguration : IEntityTypeConfiguration<UserRefreshToken>
    {
        public void Configure(EntityTypeBuilder<UserRefreshToken> entity)
        {
            entity.ToTable("UserRefreshTokens", "auth");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Token)
                  .HasMaxLength(255)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.JwtId)
                  .HasMaxLength(100)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.ExpiresAt).IsRequired();

            entity.Property(e => e.Version).IsRowVersion();

            entity.HasOne<User>()
                  .WithMany()
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.Token)
                  .HasDatabaseName("IX_auth_UserRefreshTokens_Token");

            entity.HasIndex(e => e.UserId)
                  .HasDatabaseName("IX_auth_UserRefreshTokens_UserId")
                  .IncludeProperties(e => new { e.IsRevoked, e.IsUsed });
        }
    }
}
