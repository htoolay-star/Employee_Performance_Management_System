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
        [HttpPost("login")]
        public async Task<ActionResult<SuccessResponse<AuthResponse>>> Login([FromBody] LoginRequest request)
        {
            var response = await _authService.LoginAsync(request);
            return HandleResult(response);
        }

        [Authorize(Roles = RoleConstants.SystemAdmin)]
        [HttpPost("register")]
        public async Task<ActionResult<SuccessResponse<AuthResponse>>> Register([FromBody] CreateUserRequest request)
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
        public async Task<ActionResult<SuccessResponse>> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var userGuidClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userGuidClaim, out var userGuid))
            {
                return HandleResult(SuccessResponse.Fail("Invalid user token.", ErrorType.Unauthorized));
            }

            var response = await _authService.ChangePasswordAsync(userGuid, request);
            return HandleResult(response);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<ActionResult<SuccessResponse>> Logout([FromBody] LogoutRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.RefreshToken))
            {
                return HandleResult(SuccessResponse.Fail("Refresh token is required.", ErrorType.Validation));
            }

            var response = await _authService.LogoutAsync(request.RefreshToken);
            return HandleResult(response);
        }

        [Authorize(Roles = RoleConstants.Admin)]
        [HttpPut("default-password")]
        public async Task<ActionResult<SuccessResponse>> UpdateDefaultPassword([FromBody] UpdateDefaultPasswordRequest request)
        {
            await _settingsService.UpdateDefaultPasswordAsync(request.NewDefaultPassword);
            return Ok(new { message = "Default password updated successfully." });
        }
    }
}
