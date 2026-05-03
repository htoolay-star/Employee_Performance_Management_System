using EPMS.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Entities.Performance
{
    public class KPIWeightPriority : AuditableEntity , ISoftDeletable
    {
        private KPIWeightPriority() { }

        public KPIWeightPriority(string levelName, decimal minWeight, decimal maxWeight, string? colorCode = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(levelName);

            if (minWeight > maxWeight)
            {
                throw new ArgumentException("MinWeight cannot be greater than MaxWeight.");
            }

            LevelName = levelName.Trim();
            MinWeight = minWeight;
            MaxWeight = maxWeight;
            ColorCode = colorCode?.Trim().ToUpperInvariant();
            IsActive = true;
        }

        public string LevelName { get; private set; } = string.Empty;
        public decimal MinWeight { get; private set; }
        public decimal MaxWeight { get; private set; }
        public string? ColorCode { get; private set; }
        public bool IsActive { get; private set; }

        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }

        public byte[] Version { get; private set; } = Array.Empty<byte>();

        public bool IsValidWeight(decimal weight) => weight >= MinWeight && weight <= MaxWeight;
    }
}
