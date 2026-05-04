using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Auth;
using System;

namespace EPMS.Domain.Entities.Shared
{
    public class DocumentAttachment : AuditableEntity, ISoftDeletable
    {
        private DocumentAttachment() { }

        public DocumentAttachment(
            string entityType,
            long entityId,
            string fileName,
            string filePath,
            long fileSize,
            string mimeType,
            long uploadedById,
            string? description = null,
            string? category = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(entityType);
            ArgumentException.ThrowIfNullOrWhiteSpace(fileName);
            ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
            ArgumentException.ThrowIfNullOrWhiteSpace(mimeType);

            if (entityId <= 0)
                throw new ArgumentException("EntityId must be greater than 0.");

            if (fileSize <= 0)
                throw new ArgumentException("FileSize must be greater than 0.");

            EntityType = entityType.Trim();
            EntityId = entityId;
            FileName = fileName.Trim();
            FilePath = filePath.Trim();
            FileSize = fileSize;
            MimeType = mimeType.Trim().ToLowerInvariant();
            UploadedById = uploadedById;
            Description = description?.Trim();
            Category = category?.Trim().ToUpperInvariant() ?? "GENERAL";

            IsDeleted = false;
        }

        public string EntityType { get; private set; } = string.Empty;
        public long EntityId { get; private set; }

        public string FileName { get; private set; } = string.Empty;
        public string FilePath { get; private set; } = string.Empty;
        public long FileSize { get; private set; }
        public string MimeType { get; private set; } = string.Empty;

        public string? Description { get; private set; }
        public string Category { get; private set; } = string.Empty;

        public long UploadedById { get; private set; }
        public DateTimeOffset UploadedAt { get; private set; } = DateTimeOffset.UtcNow;

        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }

        public byte[] Version { get; private set; } = Array.Empty<byte>();

        public virtual User UploadedBy { get; private set; } = null!;

        public void UpdateDescription(string? description)
        {
            Description = description?.Trim();
        }

        public void UpdateCategory(string category)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(category);
            Category = category.Trim().ToUpperInvariant();
        }
    }
}
