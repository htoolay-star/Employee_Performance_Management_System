using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Auth;
using EPMS.Domain.Interface.IService.App;
using EPMS.Domain.Interface.IService.Auth;
using EPMS.Shared.Constants;
using EPMS.Shared.DTOs.Auth;
using EPMS.Shared.DTOs.AuthDTOs;
using EPMS.Shared.DTOs.Common;
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

        // Cache TTL constants
        private static readonly TimeSpan UserCacheTtl = TimeSpan.FromMinutes(5);
        private static readonly TimeSpan RolesCacheTtl = TimeSpan.FromHours(1);

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

        /// <summary>
        /// Gets user by email with caching. Returns cached user if available and active.
        /// </summary>
        private async Task<User?> GetCachedUserByEmailAsync(string email)
        {
            var cacheKey = CacheKeys.UserByEmail(email);

            // Try cache first
            var cached = await _cacheService.GetAsync<CachedUserInfo>(cacheKey);
            if (cached != null && cached.IsActive)
            {
                // Reconstruct user from cache
                var role = new Role(0, cached.RoleName ?? RoleConstants.User, null);
                var user = new User(cached.Email, cached.PasswordHash, Enum.Parse<UserRole>(cached.RoleName ?? nameof(UserRole.User)));

                // Set private fields via reflection
                typeof(User).GetProperty("Id")?.SetValue(user, cached.Id);
                typeof(User).GetProperty("PublicId")?.SetValue(user, cached.PublicId);
                typeof(User).GetProperty("IsActive")?.SetValue(user, cached.IsActive);
                typeof(User).GetProperty("IsFirstLogin")?.SetValue(user, cached.IsFirstLogin);
                typeof(User).GetProperty("LastLoginDate")?.SetValue(user, cached.LastLoginDate);

                return user;
            }

            // Cache miss - fetch from DB
            var userFromDb = await _unitOfWork.Auth.Users.GetByEmailAsync(email, trackChanges: true);

            if (userFromDb != null)
            {
                // Store in cache
                var userInfo = new CachedUserInfo
                {
                    Id = userFromDb.Id,
                    PublicId = userFromDb.PublicId,
                    Email = userFromDb.Email,
                    RoleName = userFromDb.Role.Name,
                    IsActive = userFromDb.IsActive,
                    IsFirstLogin = userFromDb.IsFirstLogin,
                    LastLoginDate = userFromDb.LastLoginDate,
                    PasswordHash = userFromDb.PasswordHash
                };
                await _cacheService.SetAsync(cacheKey, userInfo, UserCacheTtl);
            }

            return userFromDb;
        }

        /// <summary>
        /// Updates user cache after successful login or user update.
        /// </summary>
        private async Task UpdateUserCacheAsync(User user)
        {
            var cacheKey = CacheKeys.UserByEmail(user.Email);
            var userInfo = new CachedUserInfo
            {
                Id = user.Id,
                PublicId = user.PublicId,
                Email = user.Email,
                RoleName = user.Role.Name,
                IsActive = user.IsActive,
                IsFirstLogin = user.IsFirstLogin,
                LastLoginDate = user.LastLoginDate,
                PasswordHash = user.PasswordHash
            };
            await _cacheService.SetAsync(cacheKey, userInfo, UserCacheTtl);
        }

        public async Task<SuccessResponse<AuthResponse>> LoginAsync(LoginRequest request)
        {
            // Use cached user lookup for better performance
            var user = await GetCachedUserByEmailAsync(request.Email);

            if (user == null || !user.IsActive)
            {
                return SuccessResponse<AuthResponse>.Fail("Invalid email or password.", ErrorType.Unauthorized);
            }

            var isPasswordValid = _passwordHasher.Verify(request.Password, user.PasswordHash);

            if (!isPasswordValid)
            {
                return SuccessResponse<AuthResponse>.Fail("Invalid email or password.", ErrorType.Unauthorized);
            }

            var jwtId = Guid.NewGuid().ToString();

            // Use cached roles for better performance
            var roles = await GetUserRolesAsync(user.Id);

            var userInfo = new ITokenService.TokenUserInfo(
                user.Id,
                user.Email,
                roles,
                jwtId,
                user.IsFirstLogin
            );

            var accessToken = _tokenService.GenerateAccessToken(userInfo);
            var refreshToken = _tokenService.GenerateRefreshToken();

            var expiry = _timeProvider.GetUtcNow().AddDays(_jwtSettings.RefreshTokenExpirationDays);
            user.AddRefreshToken(refreshToken, jwtId, _timeProvider, expiry);

            user.UpdateLastLogin(_timeProvider);

            await _unitOfWork.CompleteAsync();

            // Update cache with latest user data
            await UpdateUserCacheAsync(user);

            var authData = new AuthResponse
            {
                Tokens = new TokenResponse
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    RefreshTokenExpiration = expiry
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

            return SuccessResponse<AuthResponse>.Ok(authData, "Login successful");
        }

        public async Task<SuccessResponse<AuthResponse>> RegisterAsync(CreateUserRequest request)
        {
            var userAlreadyExists = await _unitOfWork.Auth.Users.ExistsAsync(request.Email);
            if (userAlreadyExists)
            {
                return SuccessResponse<AuthResponse>.Fail("Email is already registered.", ErrorType.Conflict);
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

            var authData = new AuthResponse
            {
                Tokens = new TokenResponse
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    RefreshTokenExpiration = _timeProvider.GetUtcNow().AddDays(_jwtSettings.RefreshTokenExpirationDays)
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

            return SuccessResponse<AuthResponse>.Ok(authData, "User registered successfully");
        }

        public async Task<SuccessResponse<AuthResponse>> RefreshTokenAsync(RefreshTokenRequest request)
        {
            var storedToken = await _unitOfWork.Auth.UsersRefreshToken.GetByTokenWithUserAsync(request.RefreshToken);

            if (storedToken == null)
            {
                return SuccessResponse<AuthResponse>.Fail("Invalid refresh token.", ErrorType.Unauthorized);
            }

            if (storedToken.ExpiresAt < _timeProvider.GetUtcNow())
            {
                _unitOfWork.Auth.UsersRefreshToken.Delete(storedToken);
                await _unitOfWork.CompleteAsync();
                return SuccessResponse<AuthResponse>.Fail("Refresh token expired. Please login again.", ErrorType.Unauthorized);
            }

            var user = storedToken.User;

            var newJwtId = Guid.NewGuid().ToString();

            // Use cached roles for better performance
            var roles = await GetUserRolesAsync(user.Id);

            var userInfo = new ITokenService.TokenUserInfo(
                user.Id,
                user.Email,
                roles,
                newJwtId,
                user.IsFirstLogin
            );

            var newAccessToken = _tokenService.GenerateAccessToken(userInfo);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            _unitOfWork.Auth.UsersRefreshToken.Delete(storedToken);

            user.AddRefreshToken(newRefreshToken, newJwtId, _timeProvider, _timeProvider.GetUtcNow().AddDays(_jwtSettings.RefreshTokenExpirationDays));

            await _unitOfWork.CompleteAsync();

            var authData = new AuthResponse
            {
                Tokens = new TokenResponse
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken,
                    RefreshTokenExpiration = _timeProvider.GetUtcNow().AddDays(_jwtSettings.RefreshTokenExpirationDays)
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

            return SuccessResponse<AuthResponse>.Ok(authData, "Token refreshed successfully");
        }

        public async Task<SuccessResponse> ChangePasswordAsync(Guid userGuid, ChangePasswordRequest request)
        {
            var user = await _unitOfWork.Auth.Users.GetByIdAsync(userGuid);
            if (user == null) return SuccessResponse.Fail("User not found.", ErrorType.NotFound);

            if (!_passwordHasher.Verify(request.CurrentPassword, user.PasswordHash))
            {
                return SuccessResponse.Fail("Current password is incorrect.", ErrorType.Unauthorized);
            }

            var hashedNewPassword = _passwordHasher.Hash(request.NewPassword);

            user.ChangePassword(hashedNewPassword);

            var result = await _unitOfWork.CompleteAsync() > 0;

            // Invalidate cache when password changes
            if (result)
            {
                await InvalidateUserCacheAsync(user.Id, user.Email);
                return SuccessResponse<bool>.Ok(true, "Password changed successfully.");
            }

            return SuccessResponse.Fail("Failed to change password.");
        }

        public async Task<SuccessResponse> LogoutAsync(string refreshToken)
        {
            var storedToken = await _unitOfWork.Auth.UsersRefreshToken.GetByTokenWithUserAsync(refreshToken);
            if (storedToken != null)
            {
                var user = storedToken.User;
                _unitOfWork.Auth.UsersRefreshToken.Delete(storedToken);
                await _unitOfWork.CompleteAsync();

                // Invalidate user cache on logout
                await InvalidateUserCacheAsync(user.Id, user.Email);
            }

            return SuccessResponse.Ok("Logged out successfully.");
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

            // Cache roles
            await _cacheService.SetAsync(cacheKey, roles, RolesCacheTtl);

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
