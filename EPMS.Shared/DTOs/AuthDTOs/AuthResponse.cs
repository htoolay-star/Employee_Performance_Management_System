using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Shared.DTOs.Auth
{
    public record AuthResponse
    {
        public bool Success { get; init; }
        public string Message { get; init; } = string.Empty;
        public TokenResponse Tokens { get; init; } = new();

        public UserDto User { get; init; } = new();
    }
}
