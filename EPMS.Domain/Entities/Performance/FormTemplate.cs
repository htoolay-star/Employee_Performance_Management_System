using EPMS.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Entities.Performance
{
    public class FormTemplate : AuditableEntity , ISoftDeletable
    {
        private FormTemplate() { }

        public FormTemplate(string name, string formType, int? questionsPerEvaluation = null, bool hasYesNo = false, bool hasComment = false)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            ArgumentException.ThrowIfNullOrWhiteSpace(formType);

            Name = name.Trim();
            FormType = formType.Trim().ToUpperInvariant();
            IsActive = true;
            QuestionsPerEvaluation = questionsPerEvaluation;
            HasYesNo = hasYesNo;
            HasComment = hasComment;
        }

        public string Name { get; private set; } = string.Empty;
        public string FormType { get; private set; } = string.Empty;
        public bool IsActive { get; private set; }
        public int? QuestionsPerEvaluation { get; private set; }
        public bool HasYesNo { get; private set; }
        public bool HasComment { get; private set; }

        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
        public long? DeletedBy { get; set; }

        public byte[] Version { get; private set; } = Array.Empty<byte>();

        private readonly List<FormQuestion> _questions = new();
        public virtual IReadOnlyCollection<FormQuestion> Questions => _questions.AsReadOnly();

        public void AddQuestion(FormQuestion question)
        {
            ArgumentNullException.ThrowIfNull(question);
            _questions.Add(question);
        }

        public void Rename(string newName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(newName);
            Name = newName.Trim();
        }

        public void Update(string name, string formType, int? questionsPerEvaluation = null, bool? hasYesNo = null, bool? hasComment = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            ArgumentException.ThrowIfNullOrWhiteSpace(formType);

            Name = name.Trim();
            FormType = formType.Trim().ToUpperInvariant();
            QuestionsPerEvaluation = questionsPerEvaluation;
            if (hasYesNo.HasValue) HasYesNo = hasYesNo.Value;
            if (hasComment.HasValue) HasComment = hasComment.Value;
        }

        public void ToggleUIControls(bool hasYesNo, bool hasComment)
        {
            HasYesNo = hasYesNo;
            HasComment = hasComment;
        }

        public void Deactivate() => IsActive = false;
        public void Reactivate() => IsActive = true;
    }
}
