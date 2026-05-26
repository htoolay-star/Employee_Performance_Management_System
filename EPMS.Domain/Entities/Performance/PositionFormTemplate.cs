using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Hr;

namespace EPMS.Domain.Entities.Performance
{
    public class PositionFormTemplate : AuditableEntity, ISoftDeletable
    {
        private PositionFormTemplate() { }

        public PositionFormTemplate(long positionId, long formTemplateId, bool isMandatory = true)
        {
            PositionId = positionId;
            FormTemplateId = formTemplateId;
            IsMandatory = isMandatory;
        }

        public long PositionId { get; private set; }
        public long FormTemplateId { get; private set; }
        public bool IsMandatory { get; private set; }

        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
        public long? DeletedBy { get; set; }

        public byte[] Version { get; private set; } = Array.Empty<byte>();

        public virtual Position Position { get; private set; } = null!;
        public virtual FormTemplate FormTemplate { get; private set; } = null!;

        public void ToggleMandatory(bool isMandatory) => IsMandatory = isMandatory;
    }
}
