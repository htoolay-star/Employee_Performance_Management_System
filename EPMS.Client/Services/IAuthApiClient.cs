using EPMS.Shared.DTOs.Auth;
using EPMS.Shared.DTOs.Common;
using Refit;

namespace EPMS.Client.Services;

public interface IAuthApiClient
{
    [Post("/api/auth/login")]
    Task<SuccessResponse<AuthResponse>> Login([Body] LoginRequest request);
}