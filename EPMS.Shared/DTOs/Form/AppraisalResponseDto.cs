using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Shared.DTOs.Form
{
    public class AppraisalResponseDto
    {
        public long Id { get; set; }
        public decimal TotalScore { get; set; }
        public string Grade { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
