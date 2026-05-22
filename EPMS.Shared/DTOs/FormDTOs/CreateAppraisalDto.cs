namespace EPMS.Shared.DTOs.FormDTOs;

public record CreateAppraisalDto
{
    public long? EmployeeId { get; init; }
    public string? EntityType { get; init; }
    public long? EntityId { get; init; }
    public long CycleId { get; init; }
    public long ManagerReviewerId { get; init; }
}
