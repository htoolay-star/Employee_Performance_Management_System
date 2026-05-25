namespace EPMS.Shared.DTOs.FormDTOs;

public record PendingEvaluationDto(
    long AppraisalId,
    long? EmployeeId,
    string? EmployeeName,
    string? DepartmentName,
    string? CycleName,
    string? SelfStatus = null,
    string? CommitteeStatus = null
);
