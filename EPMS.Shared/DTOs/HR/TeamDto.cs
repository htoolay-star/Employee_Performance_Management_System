using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Shared.DTOs.HR
{
    public class TeamDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public long DepartmentId { get; set; }
        public bool IsActive { get; set; }
    }
}
