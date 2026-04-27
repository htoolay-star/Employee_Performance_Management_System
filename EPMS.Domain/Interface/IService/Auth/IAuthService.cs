using EPMS.Shared.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Interface.IService.Auth
{
    public interface IAuthService
    {
        Task<AuthResponse> LoginAsync(LoginRequest request);
        Task<AuthResponse> RegisterAsync(CreateUserRequest request);
        Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request);
    }
}
