using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Auth;
using EPMS.Domain.Interface.IService.Auth;
using EPMS.Shared.DTOs.Auth;
using EPMS.Shared.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;
        private readonly TimeProvider _timeProvider;
        private readonly JwtSettings _jwtSettings;

        public AuthService(
            IUnitOfWork unitOfWork,
            IPasswordHasher passwordHasher,
            ITokenService tokenService,
            IOptions<JwtSettings> jwtOptions,
            TimeProvider timeProvider)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _timeProvider = timeProvider;
            _jwtSettings = jwtOptions.Value;
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var user = await _unitOfWork.Auth.Users.GetByEmailAsync(request.Email, trackChanges: true);

            if (user == null || !user.IsActive)
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            var isPasswordValid = _passwordHasher.Verify(request.Password, user.PasswordHash);

            if (!isPasswordValid)
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            var jwtId = Guid.NewGuid().ToString();
            var userInfo = new ITokenService.TokenUserInfo(
                user.Id,
                user.Email,
                new List<string> { user.Role.Name },
                jwtId
            );

            var accessToken = _tokenService.GenerateAccessToken(userInfo);
            var refreshToken = _tokenService.GenerateRefreshToken();

            var expiry = _timeProvider.GetUtcNow().AddDays(_jwtSettings.RefreshTokenExpirationDays);
            user.AddRefreshToken(refreshToken, jwtId, _timeProvider, expiry);

            user.UpdateLastLogin(_timeProvider);

            await _unitOfWork.CompleteAsync();

            return new AuthResponse
            {
                Success = true,
                Message = "Login successful",
                Tokens = new TokenResponse
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                },
                User = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    RoleName = user.Role.Name
                }
            };
        }

        public async Task<AuthResponse> RegisterAsync(CreateUserRequest request)
        {
            var existingUser = await _unitOfWork.Auth.Users.GetByEmailAsync(request.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("Email is already registered.");
            }

            var setting = await _unitOfWork.Auth.SystemSettings.GetByKeyAsync("DefaultUserPassword");
            string rawPassword = setting?.Value ?? "EPMS@2026!";

            var hashedPassword = _passwordHasher.Hash(rawPassword);
            var newUser = new User(request.Email, hashedPassword, request.Role);

            _unitOfWork.Auth.Users.Add(newUser);
            await _unitOfWork.CompleteAsync();

            var jwtId = Guid.NewGuid().ToString();
            var userInfo = new ITokenService.TokenUserInfo(
                newUser.Id,
                newUser.Email,
                new List<string> { request.Role.ToString() },
                jwtId
            );

            var accessToken = _tokenService.GenerateAccessToken(userInfo);
            var refreshToken = _tokenService.GenerateRefreshToken();

            newUser.AddRefreshToken(refreshToken, jwtId, _timeProvider, _timeProvider.GetUtcNow().AddDays(_jwtSettings.RefreshTokenExpirationDays));
            await _unitOfWork.CompleteAsync();

            return new AuthResponse
            {
                Success = true,
                Message = "User registered successfully",
                Tokens = new TokenResponse
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                },
                User = new UserDto
                {
                    Id = newUser.Id,
                    Email = newUser.Email,
                    RoleName = newUser.Role.Name
                }
            };
        }

        public async Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request)
        {
            var storedToken = await _unitOfWork.Auth.UsersRefreshToken.GetByTokenWithUserAsync(request.RefreshToken);

            if (storedToken == null)
            {
                throw new UnauthorizedAccessException("Invalid refresh token.");
            }

            if (storedToken.ExpiresAt < _timeProvider.GetUtcNow())
            {
                _unitOfWork.Auth.UsersRefreshToken.Delete(storedToken);
                await _unitOfWork.CompleteAsync();
                throw new UnauthorizedAccessException("Refresh token expired. Please login again.");
            }

            var user = storedToken.User;

            var newJwtId = Guid.NewGuid().ToString();
            var userInfo = new ITokenService.TokenUserInfo(
                user.Id,
                user.Email,
                new List<string> { user.Role.Name },
                newJwtId
            );

            var newAccessToken = _tokenService.GenerateAccessToken(userInfo);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            _unitOfWork.Auth.UsersRefreshToken.Delete(storedToken);

            user.AddRefreshToken(newRefreshToken, newJwtId, _timeProvider, _timeProvider.GetUtcNow().AddDays(_jwtSettings.RefreshTokenExpirationDays));

            await _unitOfWork.CompleteAsync();

            return new AuthResponse
            {
                Success = true,
                Tokens = new TokenResponse
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken
                },
                User = new UserDto { Id = user.Id, Email = user.Email, RoleName = user.Role.Name }
            };
        }
    }
}
