using EPMS.Domain.Entities.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

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
            long uploadedById,
            string? description = null,
            string? category = null);

        IReadOnlyCollection<DocumentAttachment> CreateMultiple(
            string entityType,
            long entityId,
            IEnumerable<(string FileName, string FilePath, long FileSize, string MimeType)> files,
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
                uploadedById: uploadedById,
                description: description,
                category: category);
        }

        public IReadOnlyCollection<DocumentAttachment> CreateMultiple(
            string entityType,
            long entityId,
            IEnumerable<(string FileName, string FilePath, long FileSize, string MimeType)> files,
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
                uploadedById: uploadedById,
                description: description,
                category: category)).ToList().AsReadOnly();
        }
    }
}
