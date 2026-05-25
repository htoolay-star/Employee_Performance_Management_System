namespace EPMS.Shared.DTOs.FormDTOs;

public record EvaluationFormFillDto(
    long AppraisalId,
    string? EmployeeName,
    string? EmployeeNo,
    string? Position,
    string? Department,
    long CycleId,
    string? CycleName,
    string? Status,
    string Role,
    bool IsSubmitted,
    bool IsLocked,
    string KpiStatus,
    string SelfStatus,
    string ManagerStatus,
    string PeerStatus,
    string SubordinateStatus,
    string CommitteeStatus,
    List<EvaluationFormQuestionItem> Questions,
    decimal? TotalPoint = null,
    string? RatingLabel = null,
    string? ManagerName = null,
    string? TeamName = null,
    bool SelfLocked = false,
    bool KpiLocked = false,
    bool ThreeSixtyLocked = false,
    bool AppraisalLocked = false
);

public class EvaluationFormQuestionItem
{
    public long ResponseId { get; set; }
    public long TemplateId { get; set; }
    public string? TemplateName { get; set; }
    public long QuestionId { get; set; }
    public string? QuestionText { get; set; }
    public string? CategoryName { get; set; }
    public int Sequence { get; set; }
    public bool HasYesNo { get; set; }
    public bool HasComment { get; set; }
    public List<RatingLevelDto>? RatingLevels { get; set; }
    public int? MaxScore { get; set; }
    public bool? YesNoAnswer { get; set; }
    public int? RatingValue { get; set; }
    public string? Comment { get; set; }
}

public class RatingLevelDto
{
    public int Rating { get; set; }
    public decimal MinScore { get; set; }
    public decimal MaxScore { get; set; }
}
