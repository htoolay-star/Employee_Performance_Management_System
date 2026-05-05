using EPMS.Shared.DTOs.Auth;
using EPMS.Shared.DTOs.AuthDTOs;
using EPMS.Shared.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Interface.IService.Auth
{
    public interface IAuthService
    {
        Task<SuccessResponse<AuthResponse>> LoginAsync(LoginRequest request);
        Task<SuccessResponse<UserDto>> RegisterAsync(CreateUserRequest request);
        Task<SuccessResponse<AuthResponse>> RefreshTokenAsync(RefreshTokenRequest request);
        Task<SuccessResponse> ChangePasswordAsync(long userId, ChangePasswordRequest request);
        Task<SuccessResponse> LogoutAsync(string refreshToken);
    }
}
