using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Entities.Performance
{
    public class FormQuestion : AuditableEntity , ISoftDeletable
    {
        private FormQuestion() { }

        public FormQuestion(long templateId, string text, int sequence, bool hasYesNo = false, bool hasComment = false, long? categoryId = null, long? ratingScaleId = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(text);

            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(sequence);

            TemplateId = templateId;

            QuestionText = text.Trim();
            Sequence = sequence;

            HasYesNo = hasYesNo;
            HasComment = hasComment;
            CategoryId = categoryId;
            QuestionRatingScaleId = ratingScaleId;
        }

        public long TemplateId { get; private set; }
        public long? CategoryId { get; private set; }
        public long? QuestionRatingScaleId { get; private set; }

        public string QuestionText { get; private set; } = string.Empty;
        public int Sequence { get; private set; }

        public bool HasYesNo { get; private set; }
        public bool HasComment { get; private set; }

        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }

        public byte[] Version { get; private set; } = Array.Empty<byte>();

        private readonly List<Tag> _tags = new();
        public virtual IReadOnlyCollection<Tag> Tags => _tags.AsReadOnly();

        public virtual FormTemplate Template { get; private set; } = null!;
        public virtual Category? Category { get; private set; }
        public virtual QuestionRatingScale? RatingScale { get; private set; }

        public void UpdateDetails(string text, long? categoryId, long? ratingScaleId)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(text);

            QuestionText = text.Trim();
            CategoryId = categoryId;
            QuestionRatingScaleId = ratingScaleId;
        }

        public void ChangeSequence(int newSequence)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(newSequence);
            Sequence = newSequence;
        }

        public void ToggleUIControls(bool hasYesNo, bool hasComment)
        {
            HasYesNo = hasYesNo;
            HasComment = hasComment;
        }

        public void AddTag(Shared.Tag tag)
        {
            ArgumentNullException.ThrowIfNull(tag);

            if (!_tags.Contains(tag))
            {
                _tags.Add(tag);
            }
        }

        public void RemoveTag(Shared.Tag tag)
        {
            ArgumentNullException.ThrowIfNull(tag);

            if (_tags.Contains(tag))
            {
                _tags.Remove(tag);
            }
        }
    }
}
