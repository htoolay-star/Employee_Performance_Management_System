namespace EPMS.Shared.DTOs.Common;

public record ImportResult
{
    public int TotalRows { get; init; }
    public int SuccessCount { get; init; }
    public int ErrorCount { get; init; }
    public List<string> Errors { get; init; } = [];
}