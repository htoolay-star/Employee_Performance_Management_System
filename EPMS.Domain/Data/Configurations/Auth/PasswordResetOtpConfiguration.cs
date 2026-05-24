using EPMS.Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPMS.Domain.Data.Configurations.Auth;

public class PasswordResetOtpConfiguration : IEntityTypeConfiguration<PasswordResetOtp>
{
    public void Configure(EntityTypeBuilder<PasswordResetOtp> entity)
    {
        entity.ToTable("PasswordResetOtps", "auth");

        entity.HasKey(e => e.Id);

        entity.Property(e => e.Email)
              .HasMaxLength(256)
              .IsRequired();

        entity.Property(e => e.Otp)
              .HasMaxLength(10)
              .IsRequired()
              .IsUnicode(false);

        entity.Property(e => e.ExpiresAt).IsRequired();
        entity.Property(e => e.CreatedAt).IsRequired();
        entity.Property(e => e.IsUsed).HasDefaultValue(false).IsRequired();

        entity.HasIndex(e => new { e.Email, e.Otp })
              .HasDatabaseName("IX_auth_PasswordResetOtps_Email_Otp")
              .HasFilter("[IsUsed] = 0");
    }
}
