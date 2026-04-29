using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EPMS.Shared.DTOs.Auth
{
    public record UserDto
    {
        [JsonPropertyName("id")]
        public Guid UserGuid { get; init; }
        public string Email { get; init; } = string.Empty;
        public string RoleName { get; init; } = string.Empty;
        public bool IsActive { get; init; }
        public bool IsFirstLogin { get; init; }
        public DateTimeOffset? LastLoginDate { get; init; }
    }
}
