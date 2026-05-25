using EPMS.Api.Controllers.Common;
using EPMS.Domain.Interface.IService.App;
using EPMS.Domain.Interface.IService.Auth;
using EPMS.Shared.Constants;
using EPMS.Shared.DTOs.Auth;
using EPMS.Shared.DTOs.AuthDTOs;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace EPMS.Api.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ApiControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ISystemSettingsService _settingsService;

        public AuthController(IAuthService authService, ISystemSettingsService settingsService)
        {
            _authService = authService;
            _settingsService = settingsService;
        }

        [AllowAnonymous]
        [EnableRateLimiting("LoginPolicy")]
        [HttpPost("login")]
        public async Task<ActionResult<SuccessResponse<AuthResponse>>> Login([FromBody] LoginRequest request)
        {
            var response = await _authService.LoginAsync(request);
            return HandleResult(response);
        }

        [Authorize(Roles = RoleConstants.SystemAdmin)]
        [HttpPost("register")]
        public async Task<ActionResult<SuccessResponse<UserDto>>> Register([FromBody] CreateUserRequest request)
        {
            var response = await _authService.RegisterAsync(request);
            return HandleResult(response);
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<ActionResult<SuccessResponse<AuthResponse>>> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var response = await _authService.RefreshTokenAsync(request);
            return HandleResult(response);
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<ActionResult<SuccessResponse<AuthResponse>>> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!long.TryParse(userIdClaim, out var userId))
            {
                return HandleResult(SuccessResponse<AuthResponse>.Fail("Invalid user token.", ErrorType.Unauthorized));
            }

            var response = await _authService.ChangePasswordAsync(userId, request);
            return HandleResult(response);
        }

        [Authorize]
        [HttpGet("my-permissions")]
        public async Task<ActionResult<SuccessResponse<List<string>>>> GetMyPermissions()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!long.TryParse(userIdClaim, out var userId))
                return HandleResult(SuccessResponse<List<string>>.Fail("Invalid user token.", ErrorType.Unauthorized));

            var roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
            List<string> permissions;
            if (roles.Contains(RoleConstants.SystemAdmin) || roles.Contains(RoleConstants.Admin))
            {
                permissions = await _authService.GetAllPermissionCodesAsync();
            }
            else
            {
                permissions = await _authService.GetUserPermissionsAsync(userId);
            }

            return HandleResult(SuccessResponse<List<string>>.Ok(permissions, "Permissions retrieved."));
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<ActionResult<SuccessResponse>> Logout([FromBody] LogoutRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.RefreshToken))
            {
                return HandleResult(SuccessResponse.Fail("Refresh token is required.", ErrorType.Validation));
            }

            var jti = User.FindFirstValue(JwtRegisteredClaimNames.Jti) ?? string.Empty;
            var response = await _authService.LogoutAsync(request.RefreshToken, jti);
            return HandleResult(response);
        }

        [Authorize(Roles = RoleConstants.SA_Admin)]
        [HttpGet("admin-position")]
        public async Task<ActionResult<SuccessResponse<long?>>> GetAdminPosition()
        {
            var positionId = await _settingsService.GetAdminPositionIdAsync();
            return Ok(SuccessResponse<long?>.Ok(positionId,
                positionId.HasValue ? "Admin position retrieved." : "No admin position configured."));
        }

        [Authorize(Roles = RoleConstants.SystemAdmin)]
        [HttpPut("admin-position")]
        public async Task<ActionResult<SuccessResponse>> SetAdminPosition([FromBody] AdminPositionRequest request)
        {
            await _settingsService.SetAdminPositionIdAsync(request.PositionId);
            return HandleResult(SuccessResponse.Ok("Admin position updated successfully."));
        }

        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.SystemAdmin}")]
        [HttpPut("default-password")]
        public async Task<ActionResult<SuccessResponse>> UpdateDefaultPassword([FromBody] UpdateDefaultPasswordRequest request)
        {
            await _settingsService.UpdateDefaultPasswordAsync(request.NewDefaultPassword);
            return HandleResult(SuccessResponse.Ok("Default password updated successfully."));
        }

        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.SystemAdmin}")]
        [HttpGet("default-password")]
        public async Task<ActionResult<SuccessResponse<string>>> GetDefaultPassword()
        {
            var password = await _settingsService.GetDefaultPasswordAsync();
            return Ok(SuccessResponse<string>.Ok(password, "Default password retrieved."));
        }

        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<ActionResult<SuccessResponse>> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var response = await _authService.RequestOtpAsync(request);
            return HandleResult(response);
        }

        [AllowAnonymous]
        [HttpPost("verify-otp")]
        public async Task<ActionResult<SuccessResponse>> VerifyOtp([FromBody] VerifyOtpRequest request)
        {
            var response = await _authService.VerifyOtpAsync(request);
            return HandleResult(response);
        }

        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.SystemAdmin}")]
        [HttpGet("password-reset-requests")]
        public async Task<ActionResult<SuccessResponse<IEnumerable<PasswordResetRequestDto>>>> GetPendingResetRequests()
        {
            var response = await _authService.GetPendingResetRequestsAsync();
            return HandleResult(response);
        }

        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.SystemAdmin}")]
        [HttpPost("password-reset-requests/{id:long}/approve")]
        public async Task<ActionResult<SuccessResponse>> ApproveResetRequest(long id, [FromBody] AdminResetPasswordRequest request)
        {
            var adminIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!long.TryParse(adminIdClaim, out var adminId))
            {
                return HandleResult(SuccessResponse.Fail("Invalid admin token.", ErrorType.Unauthorized));
            }

            var response = await _authService.ApproveResetRequestAsync(id, adminId, request);
            return HandleResult(response);
        }

        [Authorize]
        [HttpGet("is-manager")]
        public async Task<ActionResult<SuccessResponse<bool>>> IsManager()
        {
            var result = await _authService.IsManagerAsync();
            return HandleResult(result);
        }

        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.SystemAdmin}")]
        [HttpPost("password-reset-requests/{id:long}/reject")]
        public async Task<ActionResult<SuccessResponse>> RejectResetRequest(long id)
        {
            var adminIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!long.TryParse(adminIdClaim, out var adminId))
            {
                return HandleResult(SuccessResponse.Fail("Invalid admin token.", ErrorType.Unauthorized));
            }

            var response = await _authService.RejectResetRequestAsync(id, adminId);
            return HandleResult(response);
        }
    }
}
