using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Shared.DTOs
{
    public class CreatePerformanceReivewDto
    {
        public Guid EmployeeId { get; set; }
        public int Rating { get; set; }
    }
}
