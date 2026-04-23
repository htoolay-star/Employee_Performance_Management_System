using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Shared.PositionDTOs
{
    public class PositionResponseDto
    {
        public int Position_ID { get; set; }
        public string Position_Name { get; set; } = "";
        public int Level_ID { get; set; }
        public int Department_ID { get; set; }
        public int? Team_ID { get; set; }
        public string Level_Code { get; set; } = "";
        public string Level_Name { get; set; } = "";
        public string Department_Name { get; set; } = "";
        public string? Team_Name { get; set; }
        public bool Is_Active { get; set; }
    }
}
