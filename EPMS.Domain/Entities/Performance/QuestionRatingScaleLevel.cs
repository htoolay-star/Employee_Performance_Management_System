using EPMS.Domain.Contracts;

namespace EPMS.Domain.Entities.Performance
{
    public class QuestionRatingScaleLevel : AuditableEntity, ISoftDeletable
    {
        private QuestionRatingScaleLevel() { }

        public QuestionRatingScaleLevel(long questionRatingScaleId, int rating, decimal minScore, decimal maxScore)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(rating);

            if (minScore > maxScore)
                throw new ArgumentException("MinScore cannot be greater than MaxScore.");

            QuestionRatingScaleId = questionRatingScaleId;
            Rating = rating;
            MinScore = minScore;
            MaxScore = maxScore;
        }

        public long QuestionRatingScaleId { get; private set; }
        public int Rating { get; private set; }
        public decimal MinScore { get; private set; }
        public decimal MaxScore { get; private set; }

        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
        public long? DeletedBy { get; set; }

        public byte[] Version { get; private set; } = Array.Empty<byte>();

        public virtual QuestionRatingScale QuestionRatingScale { get; private set; } = null!;

        public bool IsValidScore(int score)
        {
            return score >= MinScore && score <= MaxScore;
        }

        public void UpdateBounds(decimal minScore, decimal maxScore)
        {
            if (minScore > maxScore)
                throw new ArgumentException("MinScore cannot be greater than MaxScore.");

            MinScore = minScore;
            MaxScore = maxScore;
        }
    }
}
