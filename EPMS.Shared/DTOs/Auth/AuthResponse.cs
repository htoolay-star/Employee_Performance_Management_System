using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Shared.DTOs.Auth
{
    public record AuthResponse
    {
        public TokenResponse Tokens { get; init; } = new();
        public string Email { get; init; } = string.Empty;
        public bool IsFirstLogin { get; init; }
        public string Role { get; init; } = string.Empty;
    }
}
