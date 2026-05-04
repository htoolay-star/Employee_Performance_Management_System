using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Auth;
using EPMS.Domain.Interface.IService.App;
using EPMS.Domain.Interface.IService.Auth;
using EPMS.Shared.Constants;
using EPMS.Shared.DTOs.Auth;
using EPMS.Shared.DTOs.AuthDTOs;
using EPMS.Shared.Enums;
using EPMS.Shared.Models;
using Microsoft.Extensions.Options;

namespace EPMS.Domain.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;
        private readonly ISystemSettingsService _settingsService;
        private readonly ICacheService _cacheService;
        private readonly TimeProvider _timeProvider;
        private readonly JwtSettings _jwtSettings;

        public AuthService(
            IUnitOfWork unitOfWork,
            IPasswordHasher passwordHasher,
            ITokenService tokenService,
            ISystemSettingsService settingsService,
            ICacheService cacheService,
            IOptions<JwtSettings> jwtOptions,
            TimeProvider timeProvider)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _settingsService = settingsService;
            _cacheService = cacheService;
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
                jwtId,
                user.IsFirstLogin
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
                    UserGuid = user.PublicId,
                    Email = user.Email,
                    RoleName = user.Role.Name,
                    IsActive = user.IsActive,
                    IsFirstLogin = user.IsFirstLogin,
                    LastLoginDate = user.LastLoginDate
                }
            };
        }

        public async Task<AuthResponse> RegisterAsync(CreateUserRequest request)
        {
            var userAlreadyExists = await _unitOfWork.Auth.Users.ExistsAsync(request.Email);
            if (userAlreadyExists)
            {
                throw new InvalidOperationException("Email is already registered.");
            }

            var plainDefaultPassword = await _settingsService.GetDefaultPasswordAsync();
            var hashedPassword = _passwordHasher.Hash(plainDefaultPassword);

            var newUser = new User(request.Email, hashedPassword, UserRole.Admin);

            _unitOfWork.Auth.Users.Add(newUser);
            await _unitOfWork.CompleteAsync();

            var jwtId = Guid.NewGuid().ToString();
            var userInfo = new ITokenService.TokenUserInfo(
                newUser.Id,
                newUser.Email,
                new List<string> { UserRole.Admin.ToString() },
                jwtId,
                newUser.IsFirstLogin
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
                    UserGuid = newUser.PublicId,
                    Email = newUser.Email,
                    RoleName = UserRole.Admin.ToString(),
                    IsActive = newUser.IsActive,
                    IsFirstLogin = newUser.IsFirstLogin,
                    LastLoginDate = newUser.LastLoginDate
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
                newJwtId,
                user.IsFirstLogin
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
                User = new UserDto 
                {
                    UserGuid = user.PublicId,
                    Email = user.Email, 
                    RoleName = user.Role.Name,
                    IsActive = user.IsActive,
                    IsFirstLogin = user.IsFirstLogin,
                    LastLoginDate = user.LastLoginDate
                }
            };
        }

        public async Task<bool> ChangePasswordAsync(Guid userGuid, ChangePasswordRequest request)
        {
            var user = await _unitOfWork.Auth.Users.GetByIdAsync(userGuid);
            if (user == null) throw new KeyNotFoundException("User not found.");

            if (!_passwordHasher.Verify(request.CurrentPassword, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Current password is incorrect.");
            }

            var hashedNewPassword = _passwordHasher.Hash(request.NewPassword);

            user.ChangePassword(hashedNewPassword);

            return await _unitOfWork.CompleteAsync() > 0;
        }

        public async Task LogoutAsync(string refreshToken)
        {
            var storedToken = await _unitOfWork.Auth.UsersRefreshToken.GetByTokenWithUserAsync(refreshToken);
            if (storedToken != null)
            {
                _unitOfWork.Auth.UsersRefreshToken.Delete(storedToken);
                await _unitOfWork.CompleteAsync();
            }
        }

        // Caching helper methods - examples of how to use ICacheService

        /// <summary>
        /// Gets user roles from cache or database. Cached for 1 hour.
        /// </summary>
        public async Task<List<string>> GetUserRolesAsync(long userId)
        {
            var cacheKey = CacheKeys.UserRoles(userId);

            var cachedRoles = await _cacheService.GetAsync<List<string>>(cacheKey);
            if (cachedRoles != null)
                return cachedRoles;

            var user = await _unitOfWork.Auth.Users.GetByIdAsync(userId);
            if (user == null)
                return new List<string>();

            var roles = new List<string> { user.Role.Name };

            // Cache for 1 hour
            await _cacheService.SetAsync(cacheKey, roles, TimeSpan.FromHours(1));

            return roles;
        }

        /// <summary>
        /// Invalidates user cache when user data changes.
        /// Call this after user update/delete operations.
        /// </summary>
        public async Task InvalidateUserCacheAsync(long userId, string email)
        {
            await _cacheService.RemoveAsync(CacheKeys.UserById(userId));
            await _cacheService.RemoveAsync(CacheKeys.UserByEmail(email));
            await _cacheService.RemoveAsync(CacheKeys.UserRoles(userId));
            await _cacheService.RemoveAsync(CacheKeys.UserPermissions(userId));
        }

        /// <summary>
        /// Blacklists a token (for logout/all scenarios). Cached until token expiry.
        /// </summary>
        public async Task BlacklistTokenAsync(string token, TimeSpan expiration)
        {
            var cacheKey = CacheKeys.AuthTokenBlacklist(token);
            await _cacheService.SetAsync(cacheKey, true, expiration);
        }

        /// <summary>
        /// Checks if a token is blacklisted.
        /// </summary>
        public async Task<bool> IsTokenBlacklistedAsync(string token)
        {
            var cacheKey = CacheKeys.AuthTokenBlacklist(token);
            return await _cacheService.GetAsync<bool>(cacheKey);
        }
    }
}
