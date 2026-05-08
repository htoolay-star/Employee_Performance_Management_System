namespace EPMS.Shared.DTOs.FormDTOs;

public record UpdateAppraisalDto
{
    public string? Status { get; init; }
    public string? EmployeeComment { get; init; }
    public string? ManagerComment { get; init; }
    public string? RatingLabel { get; init; }
}