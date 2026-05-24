using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.EmployeeInfo;
using EPMS.Domain.Entities.Hr;
using EPMS.Shared.Constants;

namespace EPMS.Domain.Entities.Performance;

public class Appraisal : AuditableEntity , ISoftDeletable
{
    private Appraisal() { }

    public Appraisal(long employeeId, long cycleId, long managerReviewerId)
    {
        EmployeeId = employeeId;
        CycleId = cycleId;
        ManagerReviewerId = managerReviewerId;
        Status = AppraisalStatuses.Draft;
    }

    public Appraisal(string entityType, long entityId, long cycleId, long managerReviewerId)
    {
        EntityType = entityType;
        EntityId = entityId;
        CycleId = cycleId;
        ManagerReviewerId = managerReviewerId;
        Status = AppraisalStatuses.Draft;
    }

    public long? EmployeeId { get; private set; }
    public string? EntityType { get; private set; }
    public long? EntityId { get; private set; }
    public long CycleId { get; private set; }
    public long ManagerReviewerId { get; private set; }

    public string Status { get; private set; } = string.Empty;
    public string? RatingLabel { get; private set; }

    public string? EmployeeComment { get; private set; }
    public string? ManagerComment { get; private set; }
    public DateTimeOffset? ReviewDate { get; private set; }

    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    public long? DeletedBy { get; set; }

    public byte[] Version { get; private set; } = Array.Empty<byte>();
    public bool IsLocked { get; private set; }
    public DateTimeOffset? LockedAt { get; private set; }
    public DateTimeOffset? FinalizedDate { get; private set; }

    public bool SelfLocked { get; private set; }
    public bool SelfLockIsDeadline { get; private set; }
    public bool ThreeSixtyLocked { get; private set; }
    public bool ThreeSixtyLockIsDeadline { get; private set; }
    public bool AppraisalLocked { get; private set; }
    public bool AppraisalLockIsDeadline { get; private set; }

    public long? UnLockedById { get; private set; }
    public DateTimeOffset? UnLockedAt { get; private set; }
    public string? UnLockReason { get; private set; }
    public virtual EmployeeProfile? UnLockedBy { get; private set; }

    public virtual EmployeeProfile? Employee { get; private set; }
    public virtual AppraisalCycle Cycle { get; private set; } = null!;
    public virtual EmployeeProfile ManagerReviewer { get; private set; } = null!;

    public long? FinalRatingId { get; private set; }
    public virtual RatingScale? FinalRating { get; private set; }

    private readonly List<AppraisalDetail> _details = new();
    public virtual IReadOnlyCollection<AppraisalDetail> Details => _details.AsReadOnly();

    private readonly List<AppraisalRecommendation> _recommendations = new();
    public virtual IReadOnlyCollection<AppraisalRecommendation> Recommendations => _recommendations.AsReadOnly();

    private readonly List<EvaluationResponse> _responses = new();
    public virtual IReadOnlyCollection<EvaluationResponse> Responses => _responses.AsReadOnly();

    public decimal? TotalScore { get; private set; }
    public decimal? KpiScore { get; private set; }
    public decimal? SelfScore { get; private set; }
    public decimal? ThreeSixtyScore { get; private set; }
    public decimal? AppraisalScore { get; private set; }
    public string? FormulaWeights { get; private set; }

    public void AddDetail(AppraisalDetail detail)
    {
        ArgumentNullException.ThrowIfNull(detail);
        _details.Add(detail);
    }

    public void Lock(TimeProvider timeProvider)
    {
        if (IsLocked) throw new InvalidOperationException("Appraisal is already locked.");

        IsLocked = true;
        LockedAt = timeProvider.GetUtcNow();
        Status = AppraisalStatuses.Finalized;
        FinalizedDate = timeProvider.GetUtcNow();
    }

    public void FinalizeAppraisal(
        decimal kpiScore,
        decimal selfScore,
        decimal threeSixtyScore,
        decimal appraisalScore,
        decimal kpiWeight,
        decimal selfWeight,
        decimal threeSixtyWeight,
        decimal appraisalWeight,
        RatingScale matchingScale,
        TimeProvider timeProvider)
    {
        if (IsLocked) throw new InvalidOperationException("Appraisal is already locked.");

        KpiScore = kpiScore;
        SelfScore = selfScore;
        ThreeSixtyScore = threeSixtyScore;
        AppraisalScore = appraisalScore;

        TotalScore = (kpiScore * kpiWeight / 100m)
                   + (selfScore * selfWeight / 100m)
                   + (threeSixtyScore * threeSixtyWeight / 100m)
                   + (appraisalScore * appraisalWeight / 100m);

        FormulaWeights = $"{{\"kpi\":{kpiWeight},\"self\":{selfWeight},\"threeSixty\":{threeSixtyWeight},\"appraisal\":{appraisalWeight}}}";

        FinalRatingId = matchingScale.Id;
        RatingLabel = matchingScale.Label;
        FinalizedDate = timeProvider.GetUtcNow();
        IsLocked = true;
        LockedAt = timeProvider.GetUtcNow();
        Status = AppraisalStatuses.Finalized;
    }

    public void LockSelf(bool isDeadline)
    {
        SelfLocked = true;
        SelfLockIsDeadline = isDeadline;
    }

    public void LockThreeSixty(bool isDeadline)
    {
        ThreeSixtyLocked = true;
        ThreeSixtyLockIsDeadline = isDeadline;
    }

    public void UnlockSelf()
    {
        if (!SelfLockIsDeadline)
            SelfLocked = false;
    }

    public void UnlockThreeSixty()
    {
        if (!ThreeSixtyLockIsDeadline)
            ThreeSixtyLocked = false;
    }

    public void LockAppraisal(bool isDeadline)
    {
        AppraisalLocked = true;
        AppraisalLockIsDeadline = isDeadline;
    }

    public void UnlockAppraisalLock()
    {
        if (!AppraisalLockIsDeadline)
            AppraisalLocked = false;
    }

    public void UnlockAppraisal(long adminId, string reason, TimeProvider timeProvider)
    {
        if (!IsLocked) throw new InvalidOperationException("Appraisal is not locked.");
        if (string.IsNullOrWhiteSpace(reason)) throw new ArgumentException("An unlock reason is strictly required by compliance.");

        IsLocked = false;
        Status = AppraisalStatuses.InProgress;
        UnLockedById = adminId;
        UnLockedAt = timeProvider.GetUtcNow();
        UnLockReason = reason.Trim();
    }

    public void AddRecommendation(AppraisalRecommendation recommendation)
    {
        ArgumentNullException.ThrowIfNull(recommendation);
        if (IsLocked)
            throw new InvalidOperationException("Cannot add recommendations to a locked appraisal.");
        _recommendations.Add(recommendation);
    }

    public void AddResponse(EvaluationResponse response)
    {
        ArgumentNullException.ThrowIfNull(response);
        _responses.Add(response);
    }

    public void UpdateDetails(string? status, string? employeeComment, string? managerComment, string? ratingLabel)
    {
        if (!string.IsNullOrWhiteSpace(status))
            Status = status.Trim();
        if (employeeComment != null)
            EmployeeComment = employeeComment.Trim();
        if (managerComment != null)
            ManagerComment = managerComment.Trim();
        if (!string.IsNullOrWhiteSpace(ratingLabel))
            RatingLabel = ratingLabel.Trim();
    }
}
