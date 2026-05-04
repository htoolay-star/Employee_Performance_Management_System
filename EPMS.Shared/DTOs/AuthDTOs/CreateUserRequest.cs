using EPMS.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Shared.DTOs.Auth
{
    public record CreateUserRequest
    {
        public string Email { get; init; } = string.Empty;
    }
}
