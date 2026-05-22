using EPMS.Shared.DTOs.AppDTOs;
using EPMS.Shared.DTOs.Auth;
using EPMS.Shared.DTOs.AuthDTOs;
using EPMS.Shared.DTOs.Common;
using Refit;

namespace EPMS.Client.Services.Auth;

public interface IAuthApiClient
{
    [Post("/api/auth/login")]
    Task<SuccessResponse<AuthResponse>> Login([Body] LoginRequest request);

    [Post("/api/auth/register")]
    Task<SuccessResponse<UserDto>> Register([Body] CreateUserRequest request);

    [Post("/api/auth/logout")]
    Task<SuccessResponse> Logout([Body] LogoutRequest request);

    [Post("/api/auth/change-password")]
    Task<SuccessResponse<AuthResponse>> ChangePassword([Body] ChangePasswordRequest request);

    [Put("/api/auth/default-password")]
    Task<SuccessResponse> UpdateDefaultPassword([Body] UpdateDefaultPasswordRequest request);

    [Get("/api/auth/admin-position")]
    Task<SuccessResponse<long?>> GetAdminPosition();

    [Put("/api/auth/admin-position")]
    Task<SuccessResponse> SetAdminPosition([Body] AdminPositionRequest request);

    [Post("/api/auth/forgot-password")]
    Task<SuccessResponse> ForgotPassword([Body] ForgotPasswordRequest request);

    [Post("/api/auth/verify-otp")]
    Task<SuccessResponse> VerifyOtp([Body] VerifyOtpRequest request);

    [Get("/api/auth/password-reset-requests")]
    Task<SuccessResponse<List<PasswordResetRequestDto>>> GetPendingResetRequests();

    [Get("/api/auth/my-permissions")]
    Task<SuccessResponse<List<string>>> GetMyPermissionsAsync();

    [Post("/api/auth/password-reset-requests/{id}/approve")]
    Task<SuccessResponse> ApproveResetRequest(long id, [Body] AdminResetPasswordRequest request);

    [Post("/api/auth/password-reset-requests/{id}/reject")]
    Task<SuccessResponse> RejectResetRequest(long id);
}