using EPMS.Domain.Contracts;
using EPMS.Shared.Constants;
using EPMS.Shared.Utilities;

namespace EPMS.Domain.Entities.Performance
{
    public class AppraisalDetail : AuditableEntity, ISoftDeletable
    {
        private AppraisalDetail() { }

        public AppraisalDetail(long appraisalId, long? kpiId, string kpiName, string? categoryName, decimal weightage, decimal? targetValue, string? scoringDirection = null, long? employeeKPIId = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(kpiName);
            ArgumentOutOfRangeException.ThrowIfNegative(weightage);

            if (!kpiId.HasValue)
                throw new ArgumentException("KPIId is required.");

            AppraisalId = appraisalId;
            KPIId = kpiId;
            EmployeeKPIId = employeeKPIId;

            KPIName = kpiName.Trim();
            CategoryName = categoryName?.Trim();
            Weightage = weightage;
            TargetValue = targetValue;
            ScoringDirection = scoringDirection ?? AppraisalConstants.ScoringDirections.HigherIsBetter;

            Score = 0;
            WeightedScore = 0;
        }

        public long AppraisalId { get; private set; }
        public long? KPIId { get; private set; }
        public long? EmployeeKPIId { get; private set; }

        public string KPIName { get; private set; } = string.Empty;
        public string? CategoryName { get; private set; }
        public decimal Weightage { get; private set; }
        public decimal? TargetValue { get; private set; }

        public decimal? ActualValue { get; private set; }
        public decimal Score { get; private set; }
        public decimal WeightedScore { get; private set; }
        public string? Remarks { get; private set; }

        public string ScoringDirection { get; private set; } = AppraisalConstants.ScoringDirections.HigherIsBetter;

        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
        public long? DeletedBy { get; set; }

        public byte[] Version { get; private set; } = Array.Empty<byte>();

        public virtual Appraisal Appraisal { get; private set; } = null!;

        public void Evaluate(decimal? actualValue, string? remarks)
        {
            ActualValue = actualValue;
            Remarks = remarks?.Trim();

            Score = KPIScoringCalculator.CalculateScore(ActualValue, TargetValue, ScoringDirection);
            WeightedScore = KPIScoringCalculator.CalculateWeightedScore(Score, Weightage);
        }
    }
}
