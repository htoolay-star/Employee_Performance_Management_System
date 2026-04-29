using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Shared.DTOs.Form
{
    public class AppraisalSubmissionDto
    {
        public long Id { get; set; }
        public long EvaluatorId { get; set; }
        public string EvaluatorRole { get; set; } = string.Empty;
        public List<AppraisalDetailDto> Details { get; set; } = new();
    }
}
