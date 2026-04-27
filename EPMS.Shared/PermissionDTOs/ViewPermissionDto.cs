using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Shared.PermissionDTOs
{
    public class PermissionDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }

        // လိုအပ်ရင် Audit data တွေကိုပါ UI မှာ ပြချင်ရင် ထည့်လို့ရပါတယ်
        public DateTimeOffset CreatedAt { get; set; }
    }
}
