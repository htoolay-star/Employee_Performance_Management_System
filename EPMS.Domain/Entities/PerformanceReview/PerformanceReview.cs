using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Entities.PerformanceReview
{
    public class PerformanceReview
    {
        public Guid Id { get; set; }

        public Guid EmployeeId { get; set; }

        public int Rating { get; set; } // 1 - 5

        public string PerformanceLevel { get; set; } = string.Empty;

        public string PromotionEligibility { get; set; } = string.Empty;

        public DateTime ReviewDate { get; set; } = DateTime.UtcNow;
    }
}
