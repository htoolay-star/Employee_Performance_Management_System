namespace EPMS.Shared.DTOs.Common;

public record ImportPreviewResult
{
    public int TotalRows { get; init; }
    public int ValidCount { get; init; }
    public int ErrorCount { get; init; }
    public List<ImportPreviewRow> Rows { get; init; } = [];
}
