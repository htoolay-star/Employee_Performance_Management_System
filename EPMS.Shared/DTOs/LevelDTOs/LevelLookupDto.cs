using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Shared.DTOs.LevelDTOs
{
    public record LevelLookupDto
    {
        public long Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
