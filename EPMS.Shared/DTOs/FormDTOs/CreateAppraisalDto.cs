namespace EPMS.Shared.DTOs.FormDTOs;

public record CreateAppraisalDto
{
    public long EmployeeId { get; init; }
    public long CycleId { get; init; }
    public long AppraiserId { get; init; }
    public string EvaluatorRole { get; init; } = string.Empty;
}