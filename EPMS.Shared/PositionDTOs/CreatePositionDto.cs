using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Shared.PositionDTOs
{
    public class CreatePositionDto
    {
        [Required(ErrorMessage = "Position name is required")]
        public string Position_Name { get; set; } = "";

        [Required]
        public int Level_ID { get; set; }

        [Required]
        public int Department_ID { get; set; }

        public int? Team_ID { get; set; }
    }
}
