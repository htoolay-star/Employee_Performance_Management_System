using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Shared.DTOs.PerformanceDTOs.KPIMasterDTOs
{
    public class KPIMasterDto
    {
        public long Id { get; init; }
        public long CategoryId { get; init; }
        public string CategoryName { get; init; } = string.Empty;
        public string Code { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public string? Description { get; init; }
        public bool IsActive { get; init; }
    }
}
