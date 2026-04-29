using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Shared.DTOs.AuthDTOs
{
    public record LogoutRequest
    {
        public string RefreshToken { get; init; } = string.Empty;
    }
}
