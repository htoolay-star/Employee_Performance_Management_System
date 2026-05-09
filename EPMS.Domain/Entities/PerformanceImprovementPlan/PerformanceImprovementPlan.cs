using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Entities.PerformanceImprovementPlan
{
    public class PerformanceImprovementPlan
    {
        public Guid Id { get; set; }

        public Guid EmployeeId { get; set; }

        public string Objectives { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Status { get; set; } = "Active"; // Active, Completed, Failed

        public List<PIPProgress> ProgressUpdates { get; set; } = new();
    }
}
