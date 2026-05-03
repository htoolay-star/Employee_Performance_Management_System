using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Hr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Entities.Performance
{
    public class PositionFormTemplate
    {
        private PositionFormTemplate() { }

        public PositionFormTemplate(long positionId, int formTemplateId, bool isMandatory = true)
        {
            PositionId = positionId;
            FormTemplateId = formTemplateId;
            IsMandatory = isMandatory;
        }

        public long PositionId { get; private set; }
        public int FormTemplateId { get; private set; }
        public bool IsMandatory { get; private set; }

        public DateTimeOffset CreatedAt { get; set; }

        public byte[] Version { get; private set; } = Array.Empty<byte>();

        public virtual Position Position { get; private set; } = null!;
        public virtual FormTemplate FormTemplate { get; private set; } = null!;

        public void ToggleMandatory(bool isMandatory) => IsMandatory = isMandatory;
    }
}
