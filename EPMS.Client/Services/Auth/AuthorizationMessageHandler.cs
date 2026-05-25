using EPMS.Shared.DTOs.Auth;
using EPMS.Shared.DTOs.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace EPMS.Client.Services.Auth
{
    public class AuthorizationMessageHandler : DelegatingHandler
    {
        private static readonly SemaphoreSlim RefreshLock = new(1, 1);

        private readonly TokenStorage _tokenStorage;
        private readonly NavigationManager _navigationManager;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly JsonSerializerOptions _jsonOptions;

        public AuthorizationMessageHandler(
            TokenStorage tokenStorage,
            NavigationManager navigationManager,
            IHttpClientFactory httpClientFactory,
            AuthenticationStateProvider authStateProvider)
        {
            _tokenStorage = tokenStorage;
            _navigationManager = navigationManager;
            _httpClientFactory = httpClientFactory;
            _authStateProvider = authStateProvider;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _tokenStorage.GetAccessTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                // Don't try to refresh for anonymous endpoints
                var path = request.RequestUri?.AbsolutePath ?? "";
                if (path.EndsWith("/api/auth/login") ||
                    path.EndsWith("/api/auth/forgot-password") ||
                    path.EndsWith("/api/auth/verify-otp") ||
                    path.EndsWith("/api/auth/refresh-token"))
                {
                    if (path.EndsWith("/api/auth/refresh-token"))
                        await ForceLogoutAsync();
                    return response;
                }

                if (await TryRefreshTokenAsync(token, cancellationToken))
                {
                    var newToken = await _tokenStorage.GetAccessTokenAsync();
                    var retryRequest = await CloneRequestAsync(request, cancellationToken);
                    retryRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", newToken);
                    response = await base.SendAsync(retryRequest, cancellationToken);
                }
                else
                {
                    await ForceLogoutAsync();
                }
            }

            return response;
        }

        private async Task<bool> TryRefreshTokenAsync(string failedAccessToken, CancellationToken ct)
        {
            await RefreshLock.WaitAsync(ct);
            try
            {
                var currentAccessToken = await _tokenStorage.GetAccessTokenAsync();
                if (currentAccessToken != failedAccessToken)
                {
                    return true;
                }

                var refreshToken = await _tokenStorage.GetRefreshTokenAsync();
                if (string.IsNullOrEmpty(refreshToken))
                    return false;

                var client = _httpClientFactory.CreateClient("RefreshClient");
                var request = new RefreshTokenRequest { RefreshToken = refreshToken };

                var response = await client.PostAsJsonAsync("/api/auth/refresh-token", request, _jsonOptions, ct);

                if (!response.IsSuccessStatusCode)
                    return false;

                var body = await response.Content.ReadAsStringAsync(ct);
                var result = JsonSerializer.Deserialize<SuccessResponse<AuthResponse>>(body, _jsonOptions);

                if (result?.Success != true || result.Data == null)
                    return false;

                await _tokenStorage.SetTokensAsync(
                    result.Data.Tokens.AccessToken,
                    result.Data.Tokens.RefreshToken);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AuthHandler] Refresh Token Failed: {ex.Message}");
                return false;
            }
            finally
            {
                RefreshLock.Release();
            }
        }

        private async Task ForceLogoutAsync()
        {
            await _tokenStorage.ClearTokensAsync();
            ((JwtAuthenticationStateProvider)_authStateProvider).MarkUserAsLoggedOut();
            _navigationManager.NavigateTo("/login");
        }

        private static async Task<HttpRequestMessage> CloneRequestAsync(HttpRequestMessage request, CancellationToken ct)
        {
            var clone = new HttpRequestMessage(request.Method, request.RequestUri);

            if (request.Content != null)
            {
                var contentBytes = await request.Content.ReadAsByteArrayAsync(ct);
                clone.Content = new ByteArrayContent(contentBytes);
                foreach (var header in request.Content.Headers)
                {
                    clone.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
            }

            foreach (var header in request.Headers)
            {
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return clone;
        }
    }
}
