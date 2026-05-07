namespace EPMS.Shared.DTOs.SharedDTOs;

public record CreateDocumentAttachmentDto
{
    public string EntityType { get; init; } = string.Empty;
    public long EntityId { get; init; }
    public string FileName { get; init; } = string.Empty;
    public string FilePath { get; init; } = string.Empty;
    public long FileSize { get; init; }
    public string MimeType { get; init; } = string.Empty;
    public long UploadedById { get; init; }
    public string? Description { get; init; }
    public string? Category { get; init; }
}