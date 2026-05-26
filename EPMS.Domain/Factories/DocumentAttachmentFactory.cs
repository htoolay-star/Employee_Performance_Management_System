using EPMS.Domain.Entities.Shared;

namespace EPMS.Domain.Factories
{
    public interface IDocumentAttachmentFactory
    {
        DocumentAttachment Create(
            string entityType,
            long entityId,
            string fileName,
            string filePath,
            long fileSize,
            string mimeType,
            TimeProvider timeProvider,
            long uploadedById,
            string? description = null,
            string? category = null);

        IReadOnlyCollection<DocumentAttachment> CreateMultiple(
            string entityType,
            long entityId,
            IEnumerable<(string FileName, string FilePath, long FileSize, string MimeType)> files,
            TimeProvider timeProvider,
            long uploadedById,
            string? description = null,
            string? category = null);
    }

    public sealed class DocumentAttachmentFactory : IDocumentAttachmentFactory
    {
        public DocumentAttachment Create(
            string entityType,
            long entityId,
            string fileName,
            string filePath,
            long fileSize,
            string mimeType,
            TimeProvider timeProvider,
            long uploadedById,
            string? description = null,
            string? category = null)
        {
            return new DocumentAttachment(
                entityType: entityType,
                entityId: entityId,
                fileName: fileName,
                filePath: filePath,
                fileSize: fileSize,
                mimeType: mimeType,
                timeProvider: timeProvider,
                uploadedById: uploadedById,
                description: description,
                category: category);
        }

        public IReadOnlyCollection<DocumentAttachment> CreateMultiple(
            string entityType,
            long entityId,
            IEnumerable<(string FileName, string FilePath, long FileSize, string MimeType)> files,
            TimeProvider timeProvider,
            long uploadedById,
            string? description = null,
            string? category = null)
        {
            return files.Select(f => Create(
                entityType: entityType,
                entityId: entityId,
                fileName: f.FileName,
                filePath: f.FilePath,
                fileSize: f.FileSize,
                mimeType: f.MimeType,
                timeProvider: timeProvider,
                uploadedById: uploadedById,
                description: description,
                category: category)).ToList().AsReadOnly();
        }
    }
}
