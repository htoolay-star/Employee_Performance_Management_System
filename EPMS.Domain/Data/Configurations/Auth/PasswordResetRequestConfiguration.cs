using EPMS.Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPMS.Domain.Data.Configurations.Auth;

public class PasswordResetRequestConfiguration : IEntityTypeConfiguration<PasswordResetRequest>
{
    public void Configure(EntityTypeBuilder<PasswordResetRequest> entity)
    {
        entity.ToTable("PasswordResetRequests", "auth");

        entity.HasKey(e => e.Id);

        entity.Property(e => e.Email)
              .HasMaxLength(256)
              .IsRequired();

        entity.Property(e => e.Status)
              .HasMaxLength(20)
              .IsRequired()
              .HasConversion<string>();

        entity.Property(e => e.RequestedAt).IsRequired();
        entity.Property(e => e.RejectionReason).HasMaxLength(500);

        entity.HasOne(e => e.User)
              .WithMany()
              .HasForeignKey(e => e.UserId)
              .OnDelete(DeleteBehavior.Cascade);

        entity.HasIndex(e => e.Status)
              .HasDatabaseName("IX_auth_PasswordResetRequests_Status");
    }
}
