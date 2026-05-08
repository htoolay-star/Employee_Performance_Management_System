namespace EPMS.Shared.DTOs.FormDTOs;

public record AppraisalRecommendationDto(
    long Id,
    long AppraisalId,
    string? AppraisalEmployeeName,
    string RecommendationType,
    string Reason,
    string? ProposedValue,
    string Priority,
    string Status,
    string? HRComments,
    long? ProcessedById,
    string? ProcessedByName,
    DateTimeOffset? ActionDate,
    DateTimeOffset CreatedAt);
