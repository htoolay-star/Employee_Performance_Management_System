namespace EPMS.Shared.DTOs.RecycleBin;

public class RecycleBinItemDto
{
    public string EntityType { get; set; } = string.Empty;
    public long EntityId { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public DateTimeOffset DeletedAt { get; set; }
    public long? DeletedBy { get; set; }
    public int DaysRemaining => Math.Max(0, 30 - (int)(DateTimeOffset.UtcNow - DeletedAt).TotalDays);
}
