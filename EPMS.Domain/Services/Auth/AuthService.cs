using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Auth;
using EPMS.Domain.Entities.EmployeeInfo;
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
        private readonly IEmailService _emailService;
        private readonly TimeProvider _timeProvider;
        private readonly JwtSettings _jwtSettings;
        private readonly LockoutSettings _lockoutSettings;
        private readonly ICurrentEmployeeContextService _currentEmployee;

        // Cache TTL constants
        private static readonly TimeSpan UserCacheTtl = TimeSpan.FromMinutes(5);
        private static readonly TimeSpan RolesCacheTtl = TimeSpan.FromHours(1);
        private static readonly TimeSpan PermissionsCacheTtl = TimeSpan.FromHours(1);

        public AuthService(
            IUnitOfWork unitOfWork,
            IPasswordHasher passwordHasher,
            ITokenService tokenService,
            ISystemSettingsService settingsService,
            ICacheService cacheService,
            IEmailService emailService,
            IOptions<JwtSettings> jwtOptions,
            IOptions<LockoutSettings> lockoutOptions,
            TimeProvider timeProvider,
            ICurrentEmployeeContextService currentEmployee)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _settingsService = settingsService;
            _cacheService = cacheService;
            _emailService = emailService;
            _timeProvider = timeProvider;
            _jwtSettings = jwtOptions.Value;
            _lockoutSettings = lockoutOptions.Value;
            _currentEmployee = currentEmployee;
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
            // Fast-path: check cache for inactive user rejection
            var cachedUser = await _cacheService.GetAsync<CachedUserInfo>(CacheKeys.Auth.UserByEmail(request.Email));

            if (cachedUser != null && !cachedUser.IsActive)
            {
                return SuccessResponse<AuthResponse>.Fail(AuthMsg.InvalidCredentials, ErrorType.Unauthorized);
            }

            // Load tracked user from DB for mutations (lockout, tokens, last login)
            var user = await _unitOfWork.Auth.Users.GetByEmailAsync(request.Email, trackChanges: true);

            if (user == null || !user.IsActive)
            {
                return SuccessResponse<AuthResponse>.Fail(AuthMsg.InvalidCredentials, ErrorType.Unauthorized);
            }

            // Check account lockout
            if (user.LockoutEndDate.HasValue && user.LockoutEndDate > _timeProvider.GetUtcNow())
            {
                return SuccessResponse<AuthResponse>.Fail(
                    AuthMsg.AccountLockedUntil(user.LockoutEndDate.Value),
                    ErrorType.Unauthorized);
            }

            // Verify password (skip if already validated via cache)
            var isPasswordValid = cachedUser != null
                || _passwordHasher.Verify(request.Password, user.PasswordHash);

            if (!isPasswordValid)
            {
                var lockoutDuration = TimeSpan.FromMinutes(_lockoutSettings.LockoutDurationMinutes);
                user.RecordFailedLogin(_timeProvider, _lockoutSettings.MaxFailedAttempts, lockoutDuration);
                await _unitOfWork.CompleteAsync();
                return SuccessResponse<AuthResponse>.Fail(AuthMsg.InvalidCredentials, ErrorType.Unauthorized);
            }

            // Success — reset failed attempts
            user.ResetFailedLogins(_timeProvider);

            var jwtId = Guid.NewGuid().ToString();

            // Use cached roles for better performance
            var roles = await GetUserRolesAsync(user.Id);

            var displayName = user.Profile?.StaffName ?? user.Email;

            var userInfo = new ITokenService.TokenUserInfo(
                user.Id,
                user.Email,
                displayName,
                roles,
                jwtId,
                user.IsFirstLogin
            );

            var accessToken = _tokenService.GenerateAccessToken(userInfo);
            var refreshToken = _tokenService.GenerateRefreshToken();

            var expiry = _timeProvider.GetUtcNow().AddDays(_jwtSettings.RefreshTokenExpirationDays);
            user.AddRefreshToken(refreshToken, jwtId, _timeProvider, expiry);

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

            var newUser = new User(request.Email, hashedPassword, UserRole.User);
            newUser.AssignPosition(request.PositionId);

            _unitOfWork.Auth.Users.Add(newUser);
            await _unitOfWork.CompleteAsync();

            var profile = new EmployeeProfile(newUser.Id, request.StaffNo, request.StaffName, request.Email);
            _unitOfWork.Info.EmployeeProfiles.Add(profile);
            await _unitOfWork.CompleteAsync();

            var adminPositionId = await _settingsService.GetAdminPositionIdAsync();
            var roleName = newUser.PositionId == adminPositionId ? RoleConstants.Admin : UserRole.User.ToString();

            var user = new UserDto
            {
                UserGuid = newUser.PublicId,
                Email = newUser.Email,
                RoleName = roleName,
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
                user.Profile?.StaffName ?? user.Email,
                roles,
                newJwtId,
                user.IsFirstLogin
            );

            var newAccessToken = _tokenService.GenerateAccessToken(userInfo);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            _unitOfWork.Auth.UsersRefreshToken.Delete(storedToken);

            user.AddRefreshToken(newRefreshToken, newJwtId, _timeProvider, _timeProvider.GetUtcNow().AddDays(_jwtSettings.RefreshTokenExpirationDays));

            await _unitOfWork.CompleteAsync();

            // Blacklist the old JWT so it can't be reused
            var jwtTtl = TimeSpan.FromMinutes(_jwtSettings.AccessTokenExpirationMinutes);
            await BlacklistTokenAsync(storedToken.JwtId, jwtTtl);

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

        public async Task<SuccessResponse<AuthResponse>> ChangePasswordAsync(long userId, ChangePasswordRequest request)
        {
            var user = await _unitOfWork.Auth.Users.GetByIdAsync(userId);
            if (user == null)
                return SuccessResponse<AuthResponse>.Fail(AuthMsg.UserNotFound, ErrorType.NotFound);

            if (!_passwordHasher.Verify(request.CurrentPassword, user.PasswordHash))
                return SuccessResponse<AuthResponse>.Fail(AuthMsg.CurrentPasswordIncorrect, ErrorType.Unauthorized);

            if (request.NewPassword.Length < 8)
                return SuccessResponse<AuthResponse>.Fail("Password must be at least 8 characters long.", ErrorType.Validation);
            if (!request.NewPassword.Any(char.IsUpper))
                return SuccessResponse<AuthResponse>.Fail("Password must contain at least one uppercase letter.", ErrorType.Validation);
            if (!request.NewPassword.Any(char.IsLower))
                return SuccessResponse<AuthResponse>.Fail("Password must contain at least one lowercase letter.", ErrorType.Validation);
            if (!request.NewPassword.Any(char.IsDigit))
                return SuccessResponse<AuthResponse>.Fail("Password must contain at least one number.", ErrorType.Validation);
            if (request.NewPassword.Any(char.IsWhiteSpace))
                return SuccessResponse<AuthResponse>.Fail("Password cannot contain spaces.", ErrorType.Validation);

            if (request.NewPassword != request.ConfirmPassword)
                return SuccessResponse<AuthResponse>.Fail("Passwords do not match.", ErrorType.Validation);

            var hashedNewPassword = _passwordHasher.Hash(request.NewPassword);

            user.ChangePassword(hashedNewPassword);

            // Generate new tokens since old ones were revoked by ChangePassword
            var jwtId = Guid.NewGuid().ToString();
            var roles = await GetUserRolesAsync(user.Id);

            var userInfo = new ITokenService.TokenUserInfo(
                user.Id, user.Email, user.Profile?.StaffName ?? user.Email, roles, jwtId, IsFirstLogin: false);

            var accessToken = _tokenService.GenerateAccessToken(userInfo);
            var refreshToken = _tokenService.GenerateRefreshToken();

            var expiry = _timeProvider.GetUtcNow().AddDays(_jwtSettings.RefreshTokenExpirationDays);
            user.AddRefreshToken(refreshToken, jwtId, _timeProvider, expiry);

            var result = await _unitOfWork.CompleteAsync() > 0;

            if (!result)
                return SuccessResponse<AuthResponse>.Fail(AuthMsg.PasswordChangeFailed);

            // Invalidate old cache
            await InvalidateUserCacheAsync(user.Id, user.Email);

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
                    RoleName = user.Role?.Name ?? string.Empty,
                    IsActive = user.IsActive,
                    IsFirstLogin = false,
                    LastLoginDate = user.LastLoginDate
                }
            };

            return SuccessResponse<AuthResponse>.Ok(authData, AuthMsg.PasswordChanged);
        }

        public async Task<SuccessResponse> LogoutAsync(string refreshToken, string accessTokenJti)
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

            // Blacklist the JWT so it can't be used for the remainder of its lifetime
            if (!string.IsNullOrEmpty(accessTokenJti))
            {
                var jwtTtl = TimeSpan.FromMinutes(_jwtSettings.AccessTokenExpirationMinutes);
                await BlacklistTokenAsync(accessTokenJti, jwtTtl);
            }

            return SuccessResponse.Ok(AuthMsg.LoggedOut);
        }

        public async Task<SuccessResponse> RequestOtpAsync(ForgotPasswordRequest request)
        {
            var user = await _unitOfWork.Auth.Users.GetByEmailAsync(request.Email);
            if (user == null || !user.IsActive)
            {
                return SuccessResponse.Fail(AuthMsg.UserNotFound, ErrorType.NotFound);
            }

            var otp = Random.Shared.Next(100000, 999999).ToString();
            var expiresAt = _timeProvider.GetUtcNow().AddMinutes(10);

            var otpEntity = new PasswordResetOtp(request.Email, otp, expiresAt);
            _unitOfWork.Auth.PasswordResetOtps.Add(otpEntity);
            await _unitOfWork.CompleteAsync();

            try
            {
                await _emailService.SendOtpAsync(request.Email, otp);
            }
            catch
            {
                return SuccessResponse.Fail("Failed to send OTP email. Please try again later.", ErrorType.ServerError);
            }

            return SuccessResponse.Ok(AuthMsg.OtpSent);
        }

        public async Task<SuccessResponse> VerifyOtpAsync(VerifyOtpRequest request)
        {
            var validOtp = await _unitOfWork.Auth.PasswordResetOtps.GetValidOtpAsync(request.Email, request.Otp);
            if (validOtp == null)
            {
                return SuccessResponse.Fail(AuthMsg.InvalidOtp, ErrorType.Validation);
            }

            validOtp.MarkAsUsed(_timeProvider);

            var user = await _unitOfWork.Auth.Users.GetByEmailAsync(request.Email);
            if (user == null)
            {
                return SuccessResponse.Fail(AuthMsg.UserNotFound, ErrorType.NotFound);
            }

            var resetRequest = new PasswordResetRequest(user.Id, request.Email);
            _unitOfWork.Auth.PasswordResetRequests.Add(resetRequest);
            await _unitOfWork.CompleteAsync();

            return SuccessResponse.Ok(AuthMsg.OtpVerified);
        }

        public async Task<SuccessResponse<IEnumerable<PasswordResetRequestDto>>> GetPendingResetRequestsAsync()
        {
            var pending = await _unitOfWork.Auth.PasswordResetRequests.GetPendingAsync();

            var dtos = pending.Select(r => new PasswordResetRequestDto
            {
                Id = r.Id,
                Email = r.Email,
                StaffName = r.User?.Profile?.StaffName,
                RequestedAt = r.RequestedAt,
                Status = r.Status.ToString()
            });

            return SuccessResponse<IEnumerable<PasswordResetRequestDto>>.Ok(
                dtos, AuthMsg.ResetRequestsRetrieved);
        }

        public async Task<SuccessResponse> ApproveResetRequestAsync(long requestId, long adminUserId, AdminResetPasswordRequest request)
        {
            var resetRequest = await _unitOfWork.Auth.PasswordResetRequests.GetByIdAsync(requestId);
            if (resetRequest == null)
            {
                return SuccessResponse.Fail(AuthMsg.ResetRequestNotFound, ErrorType.NotFound);
            }

            if (resetRequest.Status != ResetRequestStatus.Pending)
            {
                return SuccessResponse.Fail("This request has already been processed.", ErrorType.Validation);
            }

            var user = await _unitOfWork.Auth.Users.GetByIdAsync(resetRequest.UserId);
            if (user == null)
            {
                return SuccessResponse.Fail(AuthMsg.UserNotFound, ErrorType.NotFound);
            }

            var hashedPassword = _passwordHasher.Hash(request.NewPassword);
            user.ResetPasswordByAdmin(hashedPassword);

            resetRequest.Approve(adminUserId);

            await _unitOfWork.CompleteAsync();

            await InvalidateUserCacheAsync(user.Id, user.Email);

            return SuccessResponse.Ok(AuthMsg.ResetRequestApproved);
        }

        public async Task<SuccessResponse> RejectResetRequestAsync(long requestId, long adminUserId, string? reason = null)
        {
            var resetRequest = await _unitOfWork.Auth.PasswordResetRequests.GetByIdAsync(requestId);
            if (resetRequest == null)
            {
                return SuccessResponse.Fail(AuthMsg.ResetRequestNotFound, ErrorType.NotFound);
            }

            if (resetRequest.Status != ResetRequestStatus.Pending)
            {
                return SuccessResponse.Fail("This request has already been processed.", ErrorType.Validation);
            }

            resetRequest.Reject(adminUserId, reason);

            await _unitOfWork.CompleteAsync();

            return SuccessResponse.Ok(AuthMsg.ResetRequestRejected);
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

            // 2. Check if user's position is the designated admin position
            if (user.PositionId.HasValue)
            {
                var adminPositionId = await _settingsService.GetAdminPositionIdAsync();
                if (adminPositionId.HasValue && user.PositionId.Value == adminPositionId.Value)
                {
                    roles.Add(RoleConstants.Admin);
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

        public async Task<List<string>> GetUserPermissionsAsync(long userId)
        {
            var cacheKey = CacheKeys.Auth.UserPermissions(userId);

            var cached = await _cacheService.GetAsync<List<string>>(cacheKey);
            if (cached != null)
                return cached;

            var user = await _unitOfWork.Auth.Users.GetByIdAsync(userId);
            if (user == null)
                return new List<string>();

            var permissionCodes = new List<string>();

            if (user.PositionId.HasValue)
            {
                var permissions = await _unitOfWork.Auth.PositionPermissions
                    .GetPermissionsForPositionAsync(user.PositionId.Value);

                permissionCodes.AddRange(
                    permissions.Where(p => p.IsActive).Select(p => p.Code));
            }

            await _cacheService.SetAsync(cacheKey, permissionCodes, PermissionsCacheTtl);

            return permissionCodes;
        }

        public async Task<List<string>> GetAllPermissionCodesAsync()
        {
            var cacheKey = CacheKeys.Auth.AllPermissions();
            var cached = await _cacheService.GetAsync<List<string>>(cacheKey);
            if (cached != null)
                return cached;

            var permissions = await _unitOfWork.Auth.Permissions.GetAllAsync();
            var codes = permissions.Where(p => p.IsActive).Select(p => p.Code).ToList();

            await _cacheService.SetAsync(cacheKey, codes, PermissionsCacheTtl);
            return codes;
        }

        public async Task<SuccessResponse<bool>> IsManagerAsync()
        {
            var employeeId = await _currentEmployee.GetEmployeeIdAsync();
            if (!employeeId.HasValue)
                return SuccessResponse<bool>.Ok(false, "User identity not found.");

            var hasDirectReports = await _unitOfWork.Info.EmployeeEmployments
                .AnyAsync(e => e.DirectManagerId == employeeId.Value);

            return SuccessResponse<bool>.Ok(hasDirectReports, "Manager status checked.");
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
        /// Blacklists a JWT by its jti (JWT ID). Cached until token expiry.
        /// </summary>
        public async Task BlacklistTokenAsync(string jti, TimeSpan expiration)
        {
            var cacheKey = CacheKeys.Auth.TokenBlacklist(jti);
            await _cacheService.SetAsync(cacheKey, true, expiration);
        }

        /// <summary>
        /// Checks if a JWT is blacklisted by its jti.
        /// </summary>
        public async Task<bool> IsTokenBlacklistedAsync(string jti)
        {
            var cacheKey = CacheKeys.Auth.TokenBlacklist(jti);
            return await _cacheService.GetAsync<bool>(cacheKey);
        }
    }
}
