using EPMS.Domain.Entities.App;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Data.Configurations.App
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notifications", "app");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).UseIdentityColumn();

            builder.Property(e => e.Title).HasMaxLength(200).IsRequired();
            builder.Property(e => e.Message).IsRequired();
            builder.Property(e => e.Type).HasMaxLength(50).IsRequired();
            builder.Property(e => e.RedirectUrl).HasMaxLength(500);

            builder.Property(e => e.IsRead).HasDefaultValue(false);

            builder.HasOne(e => e.User)
                   .WithMany()
                   .HasForeignKey(e => e.ToUserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(e => e.CreatedAt).IsRequired();
            builder.Property(e => e.Version).IsRowVersion();

            builder.HasIndex(e => new { e.ToUserId, e.IsRead })
                   .HasDatabaseName("IX_Notifications_ToUserId_IsRead");
        }
    }
}
