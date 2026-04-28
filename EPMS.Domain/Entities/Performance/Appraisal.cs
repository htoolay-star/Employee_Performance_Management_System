using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.EmployeeInfo;
using EPMS.Domain.Entities.Hr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Entities.Performance
{
    public class Appraisal : IAuditableEntity
    {
        private Appraisal() { }

        public Appraisal(long employeeId, int cycleId, long appraiserId, string evaluatorRole)
        {
            EmployeeId = employeeId;
            CycleId = cycleId;
            AppraiserId = appraiserId;
            EvaluatorRole = evaluatorRole;
            Status = "Draft";
        }

        public long Id { get; private set; }
        public long EmployeeId { get; private set; }
        public int CycleId { get; private set; }
        public long AppraiserId { get; private set; }
        public string EvaluatorRole { get; private set; } = string.Empty;

        public string Status { get; private set; } = string.Empty;
        public string? RatingLabel { get; private set; }

        public string? EmployeeComment { get; private set; }
        public string? ManagerComment { get; private set; }
        public DateTimeOffset? ReviewDate { get; private set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public byte[] Version { get; private set; } = Array.Empty<byte>();
        public bool IsLocked { get; private set; }
        public DateTimeOffset? LockedAt { get; private set; }
        public DateTimeOffset? FinalizedDate { get; private set; }

        public long? UnLockedById { get; private set; }
        public DateTimeOffset? UnLockedAt { get; private set; }
        public string? UnLockReason { get; private set; }
        public virtual EmployeeProfile? UnLockedBy { get; private set; }

        public virtual EmployeeProfile Employee { get; private set; } = null!;
        public virtual AppraisalCycle Cycle { get; private set; } = null!;
        public virtual EmployeeProfile Appraiser { get; private set; } = null!;

        public int? FinalRatingId { get; private set; }
        public virtual RatingScale? FinalRating { get; private set; }

        private readonly List<AppraisalDetail> _details = new();
        public virtual IReadOnlyCollection<AppraisalDetail> Details => _details.AsReadOnly();

        private readonly List<AppraisalRecommendation> _recommendations = new();
        public virtual IReadOnlyCollection<AppraisalRecommendation> Recommendations => _recommendations.AsReadOnly();

        public decimal? TotalScore { get; private set; }

        public void CalculateTotalScore(RatingScale matchingScale)
        {
            if (Details.Any())
            {
                TotalScore = Details.Sum(d => d.WeightedScore);
                FinalRatingId = matchingScale.Id;
                RatingLabel = matchingScale.Label;
            }
        }

        public void SubmitManagerReview(string? comment)
        {
            ManagerComment = comment?.Trim();
            Status = "Completed";
            ReviewDate = DateTimeOffset.UtcNow;
        }

        private void RecalculateTotalScore()
        {
            TotalScore = _details.Sum(d => d.WeightedScore);
        }

        public void AddDetail(AppraisalDetail detail)
        {
            ArgumentNullException.ThrowIfNull(detail);

            _details.Add(detail);
            RecalculateTotalScore();
        }
        public void FinalizeAppraisal()
        {
            if (IsLocked) throw new InvalidOperationException("Appraisal is already locked.");

            Status = "Completed";
            FinalizedDate = DateTimeOffset.UtcNow;
            IsLocked = true;
            LockedAt = DateTimeOffset.UtcNow;
        }

        public void UnlockAppraisal(long adminId, string reason)
        {
            if (!IsLocked) throw new InvalidOperationException("Appraisal is not locked.");
            if (string.IsNullOrWhiteSpace(reason)) throw new ArgumentException("An unlock reason is strictly required by compliance.");

            IsLocked = false;
            Status = "In Progress";
            UnLockedById = adminId;
            UnLockedAt = DateTimeOffset.UtcNow;

            UnLockReason = reason.Trim();
        }

        public void AddRecommendation(AppraisalRecommendation recommendation)
        {
            ArgumentNullException.ThrowIfNull(recommendation);

            if (IsLocked)
                throw new InvalidOperationException("Cannot add recommendations to a locked appraisal.");

            _recommendations.Add(recommendation);
        }
    }
}
