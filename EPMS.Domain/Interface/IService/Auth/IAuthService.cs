using EPMS.Shared.DTOs.Auth;
using EPMS.Shared.DTOs.AuthDTOs;
using EPMS.Shared.DTOs.Common;

namespace EPMS.Domain.Interface.IService.Auth
{
    public interface IAuthService
    {
        Task<SuccessResponse<AuthResponse>> LoginAsync(LoginRequest request);
        Task<SuccessResponse<UserDto>> RegisterAsync(CreateUserRequest request);
        Task<SuccessResponse<AuthResponse>> RefreshTokenAsync(RefreshTokenRequest request);
        Task<SuccessResponse<AuthResponse>> ChangePasswordAsync(long userId, ChangePasswordRequest request);
        Task<SuccessResponse> LogoutAsync(string refreshToken, string accessTokenJti);
        Task<SuccessResponse> RequestOtpAsync(ForgotPasswordRequest request);
        Task<SuccessResponse> VerifyOtpAsync(VerifyOtpRequest request);
        Task<SuccessResponse<IEnumerable<PasswordResetRequestDto>>> GetPendingResetRequestsAsync();
        Task<SuccessResponse> ApproveResetRequestAsync(long requestId, long adminUserId, AdminResetPasswordRequest request);
        Task<SuccessResponse> RejectResetRequestAsync(long requestId, long adminUserId, string? reason = null);

        Task<List<string>> GetUserPermissionsAsync(long userId);
        Task<List<string>> GetAllPermissionCodesAsync();
    }
}
