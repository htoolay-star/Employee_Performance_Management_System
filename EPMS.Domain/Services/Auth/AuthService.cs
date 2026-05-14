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
using static EPMS.Shared.Constants.ServiceResponseMessages;

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
            var cacheKey = CacheKeys.Auth.UserByEmail(email);

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
            var cacheKey = CacheKeys.Auth.UserByEmail(user.Email);
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
            // Use cache for fast credential validation, then load a tracked entity for updates.
            // Reconstructing EF entities from cache is fragile (missing navigation properties, tracking, etc.).
            var cachedUser = await _cacheService.GetAsync<CachedUserInfo>(CacheKeys.Auth.UserByEmail(request.Email));

            if (cachedUser != null)
            {
                if (!cachedUser.IsActive)
                {
                    return SuccessResponse<AuthResponse>.Fail(AuthMsg.InvalidCredentials, ErrorType.Unauthorized);
                }

                var isPasswordValidCached = _passwordHasher.Verify(request.Password, cachedUser.PasswordHash);
                if (!isPasswordValidCached)
                {
                    return SuccessResponse<AuthResponse>.Fail(AuthMsg.InvalidCredentials, ErrorType.Unauthorized);
                }
            }

            // Load from DB with tracking so refresh tokens / last login updates persist.
            // If cache was empty, this is the first (and only) lookup.
            var user = await _unitOfWork.Auth.Users.GetByEmailAsync(request.Email, trackChanges: true);

            if (user == null || !user.IsActive)
            {
                return SuccessResponse<AuthResponse>.Fail(AuthMsg.InvalidCredentials, ErrorType.Unauthorized);
            }

            // If cache was empty, validate password against DB hash.
            if (cachedUser == null)
            {
                var isPasswordValid = _passwordHasher.Verify(request.Password, user.PasswordHash);
                if (!isPasswordValid)
                {
                    return SuccessResponse<AuthResponse>.Fail(AuthMsg.InvalidCredentials, ErrorType.Unauthorized);
                }
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
                    StaffName = user.Profile?.StaffName ?? string.Empty,
                    RoleName = user.Role.Name,
                    IsActive = user.IsActive,
                    IsFirstLogin = user.IsFirstLogin,
                    LastLoginDate = user.LastLoginDate
                }
            };

            return SuccessResponse<AuthResponse>.Ok(authData, AuthMsg.LoginSuccess);
        }

        public async Task<SuccessResponse<UserDto>> RegisterAsync(CreateUserRequest request)
        {
            var userAlreadyExists = await _unitOfWork.Auth.Users.ExistsAsync(request.Email);
            if (userAlreadyExists)
            {
                return SuccessResponse<UserDto>.Fail(AuthMsg.EmailAlreadyRegistered, ErrorType.Conflict);
            }

            var plainDefaultPassword = await _settingsService.GetDefaultPasswordAsync();
            var hashedPassword = _passwordHasher.Hash(plainDefaultPassword);

            var newUser = new User(request.Email, hashedPassword, UserRole.Admin);

            _unitOfWork.Auth.Users.Add(newUser);
            await _unitOfWork.CompleteAsync();

            var user = new UserDto
            {
                UserGuid = newUser.PublicId,
                Email = newUser.Email,
                RoleName = UserRole.Admin.ToString(),
                IsActive = newUser.IsActive,
                IsFirstLogin = newUser.IsFirstLogin,
                LastLoginDate = newUser.LastLoginDate
            };


            return SuccessResponse<UserDto>.Ok(user, AuthMsg.UserRegistered);
        }

        public async Task<SuccessResponse<AuthResponse>> RefreshTokenAsync(RefreshTokenRequest request)
        {
            var storedToken = await _unitOfWork.Auth.UsersRefreshToken.GetByTokenWithUserAsync(request.RefreshToken);

            if (storedToken == null)
            {
                return SuccessResponse<AuthResponse>.Fail(AuthMsg.InvalidRefreshToken, ErrorType.Unauthorized);
            }

            if (storedToken.ExpiresAt < _timeProvider.GetUtcNow())
            {
                _unitOfWork.Auth.UsersRefreshToken.Delete(storedToken);
                await _unitOfWork.CompleteAsync();
                return SuccessResponse<AuthResponse>.Fail(AuthMsg.RefreshTokenExpired, ErrorType.Unauthorized);
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

            return SuccessResponse<AuthResponse>.Ok(authData, AuthMsg.TokenRefreshed);
        }

        public async Task<SuccessResponse> ChangePasswordAsync(long userId, ChangePasswordRequest request)
        {
            var user = await _unitOfWork.Auth.Users.GetByIdAsync(userId);
            if (user == null) return SuccessResponse.Fail(AuthMsg.UserNotFound, ErrorType.NotFound);

            if (!_passwordHasher.Verify(request.CurrentPassword, user.PasswordHash))
            {
                return SuccessResponse.Fail(AuthMsg.CurrentPasswordIncorrect, ErrorType.Unauthorized);
            }

            var hashedNewPassword = _passwordHasher.Hash(request.NewPassword);

            user.ChangePassword(hashedNewPassword);

            var result = await _unitOfWork.CompleteAsync() > 0;

            // Invalidate cache when password changes
            if (result)
            {
                await InvalidateUserCacheAsync(user.Id, user.Email);
                return SuccessResponse.Ok(AuthMsg.PasswordChanged);
            }

            return SuccessResponse.Fail(AuthMsg.PasswordChangeFailed);
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

            return SuccessResponse.Ok(AuthMsg.LoggedOut);
        }

        // Caching helper methods - examples of how to use ICacheService

        /// <summary>
        /// Gets user roles from cache or database. Cached for 1 hour.
        /// Includes both direct user role and position-based roles.
        /// </summary>
        public async Task<List<string>> GetUserRolesAsync(long userId)
        {
            var cacheKey = CacheKeys.Auth.UserRoles(userId);

            var cachedRoles = await _cacheService.GetAsync<List<string>>(cacheKey);
            if (cachedRoles != null)
                return cachedRoles;

            var user = await _unitOfWork.Auth.Users.GetByIdAsync(userId);
            if (user == null)
                return new List<string>();

            var roles = new List<string>();

            // 1. Get user's direct role (fallback)
            if (!string.IsNullOrEmpty(user.Role?.Name))
            {
                roles.Add(user.Role.Name);
            }

            // 2. Get position-based roles (primary - if user has a profile with employment)
            if (user.Profile != null)
            {
                var employment = await _unitOfWork.Info.EmployeeEmployments.GetByEmployeeIdAsync(user.Profile.Id);
                if (employment != null)
                {
                    var positionRoles = await _unitOfWork.Auth.PositionRoles.GetByPositionIdAsync(employment.PositionId);
                    foreach (var pr in positionRoles.Where(pr => pr.IsActive))
                    {
                        if (!string.IsNullOrEmpty(pr.Role?.Name) && !roles.Contains(pr.Role.Name))
                        {
                            roles.Add(pr.Role.Name);
                        }
                    }
                }
            }

            // Ensure at least one role exists (fallback to User if nothing found)
            if (roles.Count == 0)
            {
                roles.Add(RoleConstants.User);
            }

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
            await _cacheService.RemoveAsync(CacheKeys.Auth.UserById(userId));
            await _cacheService.RemoveAsync(CacheKeys.Auth.UserByEmail(email));
            await _cacheService.RemoveAsync(CacheKeys.Auth.UserRoles(userId));
            await _cacheService.RemoveAsync(CacheKeys.Auth.UserPermissions(userId));
        }

        /// <summary>
        /// Blacklists a token (for logout/all scenarios). Cached until token expiry.
        /// </summary>
        public async Task BlacklistTokenAsync(string token, TimeSpan expiration)
        {
            var cacheKey = CacheKeys.Auth.TokenBlacklist(token);
            await _cacheService.SetAsync(cacheKey, true, expiration);
        }

        /// <summary>
        /// Checks if a token is blacklisted.
        /// </summary>
        public async Task<bool> IsTokenBlacklistedAsync(string token)
        {
            var cacheKey = CacheKeys.Auth.TokenBlacklist(token);
            return await _cacheService.GetAsync<bool>(cacheKey);
        }
    }
}
