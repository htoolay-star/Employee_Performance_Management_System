using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Contracts
{
    public abstract class AuditableEntity : BaseEntity, IAuditableEntity
    {
        public Guid PublicId { get; init; } = Guid.NewGuid();

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
