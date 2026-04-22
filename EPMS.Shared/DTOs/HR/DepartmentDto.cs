using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Shared.DTOs.HR
{
    public class DepartmentDto
    {
        public long Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }

        public List<TeamDto> Teams { get; set; } = new();
    }
}
