using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Shared.DTOs.Form
{
    public class AppraisalDetailDto
    {
        public long? KPIId { get; set; }
        public int? QuestionId { get; set; }
        public decimal Rating { get; set; } 
        public string? ActualValue { get; set; } 
        public string? Comment { get; set; }
    }
}
