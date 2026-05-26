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
        KpiStatus = AppraisalStatuses.Kpi.Draft;
        SelfStatus = AppraisalStatuses.Self.Draft;
        ManagerStatus = AppraisalStatuses.Manager.Draft;
        PeerStatus = AppraisalStatuses.Peer.Draft;
        SubordinateStatus = AppraisalStatuses.Subordinate.Draft;
        CommitteeStatus = AppraisalStatuses.Committee.Draft;
    }

    public Appraisal(string entityType, long entityId, long cycleId, long managerReviewerId)
    {
        EntityType = entityType;
        EntityId = entityId;
        CycleId = cycleId;
        ManagerReviewerId = managerReviewerId;
        Status = AppraisalStatuses.Draft;
        KpiStatus = AppraisalStatuses.Kpi.Draft;
        SelfStatus = AppraisalStatuses.Self.Draft;
        ManagerStatus = AppraisalStatuses.Manager.Draft;
        PeerStatus = AppraisalStatuses.Peer.Draft;
        SubordinateStatus = AppraisalStatuses.Subordinate.Draft;
        CommitteeStatus = AppraisalStatuses.Committee.Draft;
    }

    public long? EmployeeId { get; private set; }
    public string? EntityType { get; private set; }
    public long? EntityId { get; private set; }
    public long CycleId { get; private set; }
    public long ManagerReviewerId { get; private set; }

    public string Status { get; private set; } = string.Empty;
    public string KpiStatus { get; private set; } = AppraisalStatuses.Kpi.Draft;
    public string SelfStatus { get; private set; } = AppraisalStatuses.Self.Draft;
    public string ManagerStatus { get; private set; } = AppraisalStatuses.Manager.Draft;
    public string PeerStatus { get; private set; } = AppraisalStatuses.Peer.Draft;
    public string SubordinateStatus { get; private set; } = AppraisalStatuses.Subordinate.Draft;
    public string CommitteeStatus { get; private set; } = AppraisalStatuses.Committee.Draft;
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
    public bool KpiLocked { get; private set; }
    public bool KpiLockIsDeadline { get; private set; }
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

    public void UpdateOverallStatus()
    {
        bool allDone = KpiStatus == AppraisalStatuses.Kpi.Finalized
            && SelfStatus == AppraisalStatuses.Self.Finalized
            && ManagerStatus == AppraisalStatuses.Manager.Finalized
            && PeerStatus == AppraisalStatuses.Peer.Finalized
            && SubordinateStatus == AppraisalStatuses.Subordinate.Finalized
            && CommitteeStatus == AppraisalStatuses.Committee.Finalized;

        IsLocked = allDone;
        Status = allDone ? AppraisalStatuses.Finalized : AppraisalStatuses.Draft;
        if (allDone) { LockedAt = null; FinalizedDate = null; }
    }

    public bool UpdateOverallStatusIfAllDone(TimeProvider? timeProvider = null)
    {
        if (IsLocked) return false;

        bool allDone = KpiStatus == AppraisalStatuses.Kpi.Finalized
            && SelfStatus == AppraisalStatuses.Self.Finalized
            && ManagerStatus == AppraisalStatuses.Manager.Finalized
            && PeerStatus == AppraisalStatuses.Peer.Finalized
            && SubordinateStatus == AppraisalStatuses.Subordinate.Finalized
            && CommitteeStatus == AppraisalStatuses.Committee.Finalized;

        if (allDone)
        {
            IsLocked = true;
            var now = timeProvider?.GetUtcNow() ?? DateTimeOffset.UtcNow;
            LockedAt = now;
            Status = AppraisalStatuses.Finalized;
            FinalizedDate = now;
            return true;
        }

        return false;
    }

    public void SetComputedScores(
        decimal kpiScore, decimal selfScore, decimal threeSixtyScore,
        decimal appraisalScore, decimal kpiWeight, decimal selfWeight,
        decimal threeSixtyWeight, decimal appraisalWeight,
        RatingScale matchingScale)
    {
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
        KpiLocked = true;
        KpiStatus = AppraisalStatuses.Kpi.Finalized;
        SelfStatus = AppraisalStatuses.Self.Finalized;
        ManagerStatus = AppraisalStatuses.Manager.Finalized;
        PeerStatus = AppraisalStatuses.Peer.Finalized;
        SubordinateStatus = AppraisalStatuses.Subordinate.Finalized;
        CommitteeStatus = AppraisalStatuses.Committee.Finalized;
    }

    public void LockSelf(bool isDeadline)
    {
        SelfLocked = true;
        SelfLockIsDeadline = isDeadline;
        if (SelfStatus == AppraisalStatuses.Self.Draft)
            SelfStatus = AppraisalStatuses.Self.Reviewed;
    }

    public void LockKpi(bool isDeadline)
    {
        KpiLocked = true;
        KpiLockIsDeadline = isDeadline;
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

    public void UnlockKpi()
    {
        if (!KpiLockIsDeadline)
            KpiLocked = false;
    }

    public void ApproveSelf()
    {
        if (SelfStatus != AppraisalStatuses.Self.InProgress)
            throw new InvalidOperationException("Self assessment must be InProgress to approve.");
        SelfStatus = AppraisalStatuses.Self.Reviewed;
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
        if (CommitteeStatus == AppraisalStatuses.Committee.Draft)
            CommitteeStatus = AppraisalStatuses.Committee.Reviewed;
    }

    public void SetKpiStatus(string status) => KpiStatus = status;
    public void SetSelfStatus(string status) => SelfStatus = status;
    public void SetManagerStatus(string status) => ManagerStatus = status;
    public void SetPeerStatus(string status) => PeerStatus = status;
    public void SetSubordinateStatus(string status) => SubordinateStatus = status;
    public void SetCommitteeStatus(string status) => CommitteeStatus = status;

    public void UnlockAppraisalLock()
    {
        if (!AppraisalLockIsDeadline)
            AppraisalLocked = false;
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
            KpiStatus = status.Trim();
        if (employeeComment != null)
            EmployeeComment = employeeComment.Trim();
        if (managerComment != null)
            ManagerComment = managerComment.Trim();
        if (!string.IsNullOrWhiteSpace(ratingLabel))
            RatingLabel = ratingLabel.Trim();
    }
}
