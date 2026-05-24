using EPMS.Api.Authorization;
using EPMS.Domain.Interface.IService.App;
using EPMS.Shared.Constants;
using EPMS.Shared.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EPMS.Api.Extensions
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration config)
        {
            var jwtSettings = config.GetSection("JwtSettings").Get<JwtSettings>()
                ?? throw new InvalidOperationException("JwtSettings configuration is missing.");

            if (string.IsNullOrWhiteSpace(jwtSettings.Secret))
            {
                throw new InvalidOperationException("JwtSettings:Secret is required.");
            }

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                        ValidateIssuer = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidateAudience = true,
                        ValidAudience = jwtSettings.Audience,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = async context =>
                        {
                            var jti = context.Principal?.FindFirstValue(JwtRegisteredClaimNames.Jti);
                            if (!string.IsNullOrEmpty(jti))
                            {
                                var cacheService = context.HttpContext.RequestServices
                                    .GetRequiredService<ICacheService>();
                                var isBlacklisted = await cacheService.GetAsync<bool>(
                                    CacheKeys.Auth.TokenBlacklist(jti));
                                if (isBlacklisted)
                                {
                                    context.Fail("Token has been revoked.");
                                }
                            }
                        }
                    };
                });

            services.AddAuthorization();
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
            services.AddHttpContextAccessor();
            return services;
        }
    }
}
