using EPMS.Shared.DTOs.Auth;
using EPMS.Shared.DTOs.AuthDTOs;
using EPMS.Shared.DTOs.Common;
using Refit;

namespace EPMS.Client.Services.Auth;

public interface IAuthApiClient
{
    [Post("/api/auth/login")]
    Task<SuccessResponse<AuthResponse>> Login([Body] LoginRequest request);

    [Post("/api/auth/logout")]
    Task<SuccessResponse> Logout([Body] LogoutRequest request);
}