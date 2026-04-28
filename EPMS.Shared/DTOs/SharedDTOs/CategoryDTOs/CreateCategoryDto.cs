using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Shared.DTOs.SharedDTOs.CategoryDTOs
{
    public class CreateCategoryDto
    {
        public string Module { get; set; } = string.Empty; // ဥပမာ - KPI, LEAVE
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? ParentId { get; set; }
    }
}
