using EPMS.Domain.Interface.IService.App;
using EPMS.Domain.Interface.IService.Auth;
using EPMS.Shared.Constants;
using EPMS.Shared.DTOs.Auth;
using EPMS.Shared.DTOs.AuthDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EPMS.Api.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
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
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
        {
            var response = await _authService.LoginAsync(request);
            return Ok(response);
        }

        [Authorize(Roles = RoleConstants.SystemAdmin)]
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register([FromBody] CreateUserRequest request)
        {
            var response = await _authService.RegisterAsync(request);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<ActionResult<AuthResponse>> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var response = await _authService.RefreshTokenAsync(request);
            return Ok(response);
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var userGuidClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userGuidClaim, out var userGuid)) return Unauthorized();

            var result = await _authService.ChangePasswordAsync(userGuid, request);
            return result ? Ok(new { message = "Password changed successfully." }) : BadRequest();
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.RefreshToken))
            {
                return BadRequest("Refresh token is required.");
            }

            await _authService.LogoutAsync(request.RefreshToken);

            return Ok(new { message = "Logged out successfully. Session revoked." });
        }

        [Authorize(Roles = RoleConstants.Admin)]
        [HttpPut("default-password")]
        public async Task<IActionResult> UpdateDefaultPassword([FromBody] UpdateDefaultPasswordRequest request)
        {
            await _settingsService.UpdateDefaultPasswordAsync(request.NewDefaultPassword);
            return Ok(new { message = "Default password updated successfully." });
        }
    }
}
