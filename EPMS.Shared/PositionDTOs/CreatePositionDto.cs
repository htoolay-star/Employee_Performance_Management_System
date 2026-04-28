using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace EPMS.Shared.PositionDTOs
{
    public class CreatePositionDto
    {
        [Required, StringLength(100)]
        public string PositionName { get; set; } = string.Empty;

        [Range(1, int.MaxValue)]
        public int LevelId { get; set; }

        [Range(1, int.MaxValue)]
        public int DepartmentId { get; set; }

        public int? TeamId { get; set; }
    }
}
