using EPMS.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Entities.Performance
{
    public class QuestionRatingScale : IAuditableEntity
    {
        private QuestionRatingScale() { }

        public QuestionRatingScale(string name, int minScore, int maxScore)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);

            if (minScore >= maxScore)
                throw new ArgumentException("MinScore must be strictly less than MaxScore.");

            Name = name.Trim();
            MinScore = minScore;
            MaxScore = maxScore;
            IsActive = true;
        }

        public int Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public int MinScore { get; private set; }
        public int MaxScore { get; private set; }
        public bool IsActive { get; private set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public byte[] Version { get; private set; } = Array.Empty<byte>();

        public bool IsValidScore(int score)
        {
            return score >= MinScore && score <= MaxScore;
        }

        public void Rename(string newName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(newName);
            Name = newName.Trim();
        }

        public void Deactivate() => IsActive = false;
        public void Reactivate() => IsActive = true;
    }
}
