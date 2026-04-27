using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Shared.PermissionDTOs
{
    public class UpdatePermissionDto
    {
        [Required(ErrorMessage = "Need to show Name")]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}
