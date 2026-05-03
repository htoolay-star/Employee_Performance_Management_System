using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.EmployeeInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Entities.Performance
{
    public class EvaluationResponse : AuditableEntity , ISoftDeletable
    {
        private EvaluationResponse() { }

        public EvaluationResponse(long appraisalId, int templateId, int questionId, long evaluatorId, string evaluatorRole, bool isAnonymous = false)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(evaluatorRole);

            AppraisalId = appraisalId;
            TemplateId = templateId;
            QuestionId = questionId;
            EvaluatorId = evaluatorId;

            EvaluatorRole = evaluatorRole.Trim().ToUpperInvariant();

            IsAnonymous = isAnonymous;
        }

        public long AppraisalId { get; private set; }
        public int TemplateId { get; private set; }
        public int QuestionId { get; private set; }

        public long EvaluatorId { get; private set; }
        public string EvaluatorRole { get; private set; } = string.Empty;
        public bool IsAnonymous { get; private set; }

        public bool? YesNoAnswer { get; private set; }
        public int? RatingValue { get; private set; }
        public string? QuestionComment { get; private set; }

        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }

        public byte[] Version { get; private set; } = Array.Empty<byte>();

        public virtual Appraisal Appraisal { get; private set; } = null!;
        public virtual FormTemplate Template { get; private set; } = null!;
        public virtual FormQuestion Question { get; private set; } = null!;
        public virtual EmployeeProfile Evaluator { get; private set; } = null!;

        public void SetRating(int ratingValue, QuestionRatingScale scale)
        {
            if (scale == null)
                throw new InvalidOperationException("This question does not have a rating scale configured.");

            if (!scale.IsValidScore(ratingValue))
                throw new ArgumentException($"Invalid Rating! Acceptable range is {scale.MinScore} to {scale.MaxScore}. You entered {ratingValue}.");

            RatingValue = ratingValue;
        }

        public void SetYesNo(bool isYes) => YesNoAnswer = isYes;
        public void AddComment(string comment) => QuestionComment = comment?.Trim();
    }
}
