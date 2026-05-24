using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Shared.DTOs.FormDTOs
{
    public class AppraisalSubmissionDto
    {
        public long Id { get; set; }
        public List<AppraisalDetailDto> Details { get; set; } = new();
    }
}
