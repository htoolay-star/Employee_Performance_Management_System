namespace EPMS.Shared.DTOs.SharedDTOs;

public class DocumentAttachmentDto
{
    public long Id { get; set; }
    public string EntityType { get; set; } = string.Empty;
    public long EntityId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string MimeType { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Category { get; set; } = string.Empty;
    public long UploadedById { get; set; }
    public DateTimeOffset UploadedAt { get; set; }
}