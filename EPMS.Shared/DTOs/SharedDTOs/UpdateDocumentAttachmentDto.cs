namespace EPMS.Shared.DTOs.SharedDTOs;

public record UpdateDocumentAttachmentDto
{
    public string? Description { get; init; }
    public string? Category { get; init; }
}