using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Shared;

namespace EPMS.Domain.Entities.Performance
{
    public class FormQuestion : AuditableEntity, ISoftDeletable
    {
        private FormQuestion() { }

        public FormQuestion(long templateId, string text, int sequence, long? categoryId = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(text);

            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(sequence);

            TemplateId = templateId;

            QuestionText = text.Trim();
            Sequence = sequence;

            CategoryId = categoryId;
        }

        public long TemplateId { get; private set; }
        public long? CategoryId { get; private set; }

        public string QuestionText { get; private set; } = string.Empty;
        public int Sequence { get; private set; }

        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
        public long? DeletedBy { get; set; }

        public byte[] Version { get; private set; } = Array.Empty<byte>();

        public virtual FormTemplate Template { get; private set; } = null!;
        public virtual Category? Category { get; private set; }

        public void UpdateDetails(string text, long? categoryId)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(text);

            QuestionText = text.Trim();
            CategoryId = categoryId;
        }

        public void ChangeSequence(int newSequence)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(newSequence);
            Sequence = newSequence;
        }
    }
}
