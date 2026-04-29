using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Shared.DTOs.SharedDTOs.TagDTOs
{
    public class TagDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Module { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
