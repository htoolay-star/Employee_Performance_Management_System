using EPMS.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Entities.Performance
{
    public class FormTemplate : IAuditableEntity
    {
        private FormTemplate() { }

        public FormTemplate(string name, string formType)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            ArgumentException.ThrowIfNullOrWhiteSpace(formType);

            Name = name.Trim();
            FormType = formType.Trim().ToUpperInvariant();
            IsActive = true;
        }

        public int Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string FormType { get; private set; } = string.Empty;
        public bool IsActive { get; private set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
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
        public void Deactivate() => IsActive = false;
        public void Reactivate() => IsActive = true;
    }
}
