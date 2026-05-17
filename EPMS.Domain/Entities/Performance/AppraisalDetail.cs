using EPMS.Domain.Contracts;
using EPMS.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Entities.Performance
{
    public class AppraisalDetail : AuditableEntity , ISoftDeletable
    {
        private AppraisalDetail() { }

        public AppraisalDetail(long appraisalId, long? kpiId, string kpiName, string? categoryName, decimal weightage, string? targetValue, string? scoringDirection = null, long? employeeKPIId = null)
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
            TargetValue = targetValue?.Trim();
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
        public string? TargetValue { get; private set; }

        public string? ActualValue { get; private set; }
        public decimal Score { get; private set; }
        public decimal WeightedScore { get; private set; }
        public string? Remarks { get; private set; }

        public string ScoringDirection { get; private set; } = AppraisalConstants.ScoringDirections.HigherIsBetter;

        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
        public long? DeletedBy { get; set; }

        public byte[] Version { get; private set; } = Array.Empty<byte>();

        public virtual Appraisal Appraisal { get; private set; } = null!;

        public void Evaluate(string? actualValue, string? remarks)
        {
            ActualValue = actualValue?.Trim();
            Remarks = remarks?.Trim();

            if (decimal.TryParse(ActualValue, out var actualNum)
                && decimal.TryParse(TargetValue, out var targetNum) && targetNum > 0)
            {
                Score = ScoringDirection == AppraisalConstants.ScoringDirections.LowerIsBetter
                    ? Math.Min(targetNum / actualNum, 1m) * 100
                    : Math.Min(actualNum / targetNum, 1m) * 100;
            }

            WeightedScore = Math.Round((Score * Weightage) / 100m, 2);
        }
    }
}
