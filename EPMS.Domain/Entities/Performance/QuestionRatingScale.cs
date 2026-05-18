using EPMS.Domain.Contracts;

namespace EPMS.Domain.Entities.Performance
{
    public class QuestionRatingScale : AuditableEntity, ISoftDeletable
    {
        private QuestionRatingScale() { }

        public QuestionRatingScale(string name)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);

            Name = name.Trim();
            IsActive = true;
        }

        public string Name { get; private set; } = string.Empty;
        public bool IsActive { get; private set; }

        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
        public long? DeletedBy { get; set; }

        public byte[] Version { get; private set; } = Array.Empty<byte>();

        public virtual ICollection<QuestionRatingScaleLevel> Levels { get; private set; } = new List<QuestionRatingScaleLevel>();

        public void Rename(string newName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(newName);
            Name = newName.Trim();
        }

        public void SetLevels(IEnumerable<QuestionRatingScaleLevel> levels)
        {
            Levels = levels.ToList();
        }

        public void Deactivate() => IsActive = false;
        public void Reactivate() => IsActive = true;
    }
}
