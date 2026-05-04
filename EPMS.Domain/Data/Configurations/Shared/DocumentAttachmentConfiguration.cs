using EPMS.Domain.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPMS.Domain.Data.Configurations.Shared
{
    public class DocumentAttachmentConfiguration : IEntityTypeConfiguration<DocumentAttachment>
    {
        public void Configure(EntityTypeBuilder<DocumentAttachment> entity)
        {
            entity.ToTable("DocumentAttachments", "shared");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).UseIdentityColumn();
            entity.Property(e => e.PublicId).IsRequired();
            entity.HasIndex(e => e.PublicId).IsUnique().HasFilter("[IsDeleted] = 0");

            entity.Property(e => e.EntityType).HasMaxLength(100).IsRequired();
            entity.Property(e => e.EntityId).IsRequired();

            entity.Property(e => e.FileName).HasMaxLength(255).IsRequired();
            entity.Property(e => e.FilePath).HasMaxLength(500).IsRequired();
            entity.Property(e => e.FileSize).IsRequired();
            entity.Property(e => e.MimeType).HasMaxLength(100).IsRequired();

            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Category).HasMaxLength(50).IsRequired();

            entity.Property(e => e.UploadedAt).IsRequired();
            entity.Property(e => e.Version).IsRowVersion();

            entity.Property(e => e.IsDeleted).HasDefaultValue(false).IsRequired();
            entity.Property(e => e.DeletedAt);

            entity.HasOne(e => e.UploadedBy)
                  .WithMany()
                  .HasForeignKey(e => e.UploadedById)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => new { e.EntityType, e.EntityId });
            entity.HasIndex(e => e.Category);
            entity.HasIndex(e => e.UploadedAt);
            entity.HasIndex(e => e.UploadedById);
        }
    }
}
