using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace EPMS.Client.Services.Auth
{
    public class AuthorizationMessageHandler : DelegatingHandler
    {
        private readonly TokenStorage _tokenStorage;
        private readonly NavigationManager _navigationManager;

        public AuthorizationMessageHandler(TokenStorage tokenStorage, NavigationManager navigationManager)
        {
            _tokenStorage = tokenStorage;
            _navigationManager = navigationManager;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _tokenStorage.GetAccessTokenAsync();

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                // Optionally clear tokens and redirect to login if unauthorized
                await _tokenStorage.ClearTokensAsync();
                _navigationManager.NavigateTo("/login");
            }

            return response;
        }
    }
}
