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

        public Appraisal(long employeeId, int cycleId, long appraiserId)
        {
            EmployeeId = employeeId;
            CycleId = cycleId;
            AppraiserId = appraiserId;
            Status = "Draft";
        }

        public long Id { get; private set; }
        public long EmployeeId { get; private set; }
        public int CycleId { get; private set; }
        public long AppraiserId { get; private set; }

        public string Status { get; private set; } = string.Empty;
        public decimal? TotalScore { get; private set; }
        public string? RatingLabel { get; private set; }

        public string? EmployeeComment { get; private set; }
        public string? ManagerComment { get; private set; }
        public DateTimeOffset? ReviewDate { get; private set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public byte[] Version { get; private set; } = Array.Empty<byte>();

        public virtual EmployeeProfile Employee { get; private set; } = null!;
        public virtual AppraisalCycle Cycle { get; private set; } = null!;
        public virtual EmployeeProfile Appraiser { get; private set; } = null!;

        public virtual ICollection<AppraisalDetail> Details { get; private set; } = new List<AppraisalDetail>();

        public void CalculateTotalScore(RatingScale matchingScale)
        {
            if (Details.Any())
            {
                TotalScore = Details.Sum(d => d.WeightedScore);
                RatingLabel = matchingScale.Label;
            }
        }

        public void SubmitManagerReview(string? comment)
        {
            ManagerComment = comment?.Trim();
            Status = "Completed";
            ReviewDate = DateTimeOffset.UtcNow;
        }
    }
}
