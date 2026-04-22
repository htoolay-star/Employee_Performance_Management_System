using EPMS.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Entities.Hr
{
    public class RatingScale : IAuditableEntity
    {
        private RatingScale() { }

        public RatingScale(int rating, string label, decimal minScore, decimal maxScore)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(label);

            if (minScore > maxScore)
            {
                throw new ArgumentException("MinScore cannot be greater than MaxScore.");
            }

            Rating = rating;
            Label = label.Trim();
            MinScore = minScore;
            MaxScore = maxScore;
            IsActive = true;
        }

        public int Id { get; private set; }
        public int Rating { get; private set; }
        public string Label { get; private set; } = string.Empty;

        public decimal MinScore { get; private set; }
        public decimal MaxScore { get; private set; }

        public string? PerformanceLevel { get; private set; }
        public string? PromotionEligibility { get; private set; }
        public string? Description { get; private set; }

        public bool IsActive { get; private set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public byte[] Version { get; private set; } = Array.Empty<byte>();

        public bool IsMatch(decimal totalScore) => totalScore >= MinScore && totalScore <= MaxScore;

        public void UpdateDetails(string? performanceLevel, string? eligibility, string? description)
        {
            PerformanceLevel = performanceLevel?.Trim();
            PromotionEligibility = eligibility?.Trim();
            Description = description?.Trim();
        }

        public void UpdateBounds(decimal minScore, decimal maxScore)
        {
            if (minScore > maxScore)
            {
                throw new ArgumentException("MinScore cannot be greater than MaxScore.");
            }

            MinScore = minScore;
            MaxScore = maxScore;
        }

        public void Deactivate() => IsActive = false;
        public void Reactivate() => IsActive = true;
    }
}
