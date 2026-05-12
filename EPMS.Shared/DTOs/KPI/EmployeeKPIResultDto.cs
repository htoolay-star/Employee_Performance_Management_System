using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Shared.DTOs.KPI
{
    public class EmployeeKPIResultDto
    {
        public long Id { get; set; }

        public decimal TargetValue { get; set; }

        public decimal ActualValue { get; set; }

        public decimal ScorePercentage { get; set; }

        public decimal WeightedScore { get; set; }
    }
}
