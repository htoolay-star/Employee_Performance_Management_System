using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http.Headers;
using System.Text;

namespace EPMS.Client.Handlers
{
    public class GlobalApiExceptionHandler : DelegatingHandler
    {
        private readonly NavigationManager _navigationManager;
        private readonly ISnackbar _snackbar;

        public GlobalApiExceptionHandler(NavigationManager navigationManager, ISnackbar snackbar)
        {
            _navigationManager = navigationManager;
            _snackbar = snackbar;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                // Skip interception for anonymous endpoints
                var path = request.RequestUri?.AbsolutePath ?? "";
                var isAnonymous = path.EndsWith("/api/auth/login") ||
                                  path.EndsWith("/api/auth/forgot-password") ||
                                  path.EndsWith("/api/auth/verify-otp") ||
                                  path.EndsWith("/api/auth/refresh-token");

                if (!isAnonymous)
                {
                    switch (response.StatusCode)
                    {
                        case System.Net.HttpStatusCode.Forbidden:
                            var content = await response.Content.ReadAsByteArrayAsync(cancellationToken);
                            var body = Encoding.UTF8.GetString(content);
                            // Re-buffer content for downstream consumers
                            response.Content = new ByteArrayContent(content);
                            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                            if (body.Contains("must change your default password", StringComparison.OrdinalIgnoreCase))
                            {
                                _navigationManager.NavigateTo("/change-password");
                            }
                            else
                            {
                                _snackbar.Add("You do not have permission to perform this action.", Severity.Error);
                                _navigationManager.NavigateTo("/unauthorized");
                            }
                            break;

                        case System.Net.HttpStatusCode.TooManyRequests:
                            _navigationManager.NavigateTo("/login");
                            break;

                        case System.Net.HttpStatusCode.InternalServerError:
                            _snackbar.Add("A critical server error occurred. Please contact IT support.", Severity.Error);
                            break;
                    }
                }
            }

            return response;
        }
    }
}
