using EPMS.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Entities.Performance
{
    public class AppraisalDetail : IAuditableEntity , ISoftDeletable
    {
        private AppraisalDetail() { }

        public AppraisalDetail(long appraisalId, long? kpiId, string kpiName, string? categoryName, decimal weightage, string? targetValue, int? questionId = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(kpiName);
            ArgumentOutOfRangeException.ThrowIfNegative(weightage);

            AppraisalId = appraisalId;
            KPIId = kpiId;
            QuestionId = questionId;

            KPIName = kpiName.Trim();
            CategoryName = categoryName?.Trim();
            Weightage = weightage;
            TargetValue = targetValue?.Trim();

            Score = 0;
            WeightedScore = 0;
        }

        public long Id { get; private set; }
        public long AppraisalId { get; private set; }
        public long? KPIId { get; private set; }
        public int? QuestionId { get; private set; }

        public string KPIName { get; private set; } = string.Empty;
        public string? CategoryName { get; private set; }
        public decimal Weightage { get; private set; }
        public string? TargetValue { get; private set; }

        public string? ActualValue { get; private set; }
        public decimal Score { get; private set; }
        public decimal WeightedScore { get; private set; }
        public string? Remarks { get; private set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }

        public byte[] Version { get; private set; } = Array.Empty<byte>();

        public virtual Appraisal Appraisal { get; private set; } = null!;
        public virtual FormQuestion? Question { get; private set; }

        public void Evaluate(string? actualValue, decimal rawScore, string? remarks)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(rawScore);

            ActualValue = actualValue?.Trim();
            Score = rawScore;

            // E.g., if weightage is 20%, and they scored 80/100, weighted score is 16.
            WeightedScore = (rawScore * Weightage) / 100m;
            Remarks = remarks?.Trim();
        }
    }
}
