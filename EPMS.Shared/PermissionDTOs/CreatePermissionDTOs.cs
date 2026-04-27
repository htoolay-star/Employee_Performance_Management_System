using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Shared.PermissionDTOs
{
    public class CreatePermissionDto
    {
        [Required(ErrorMessage = "Need to write Permission Code")]
        public string Code { get; set; } = string.Empty;

        [Required(ErrorMessage = "Need to write Permission Name")]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}
