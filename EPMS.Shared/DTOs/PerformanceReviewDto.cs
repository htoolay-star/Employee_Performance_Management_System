using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Shared.DTOs
{
    public class PerformanceReviewDto
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public int Rating { get; set; }
        public string PerformanceLevel { get; set; } = string.Empty;
        public string PromotionEligibility { get; set; } = string.Empty;
        public DateTime ReviewDate { get; set; }
    }
}
