namespace EPMS.Shared.DTOs.FormDTOs;

public record AppraisalDto(
    long Id,
    long EmployeeId,
    string? EmployeeName,
    long CycleId,
    string? CycleName,
    long ManagerReviewerId,
    string? ManagerReviewerName,
    string Status,
    string? RatingLabel,
    decimal? TotalScore,
    string? EmployeeComment,
    string? ManagerComment,
    DateTimeOffset? ReviewDate,
    bool IsLocked,
    DateTimeOffset? LockedAt,
    DateTimeOffset? FinalizedDate,
    DateTimeOffset CreatedAt);