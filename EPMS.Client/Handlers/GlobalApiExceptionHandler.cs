using Microsoft.AspNetCore.Components;
using MudBlazor;

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
                switch (response.StatusCode)
                {
                    case System.Net.HttpStatusCode.Unauthorized:
                        _snackbar.Add("Session expired. Please log in again.", Severity.Error);
                        // TODO: LocalStorage ထဲက Token ဖျက်တဲ့ Logic ထည့်ရန်
                        _navigationManager.NavigateTo("/login");
                        break;

                    case System.Net.HttpStatusCode.Forbidden:
                        _snackbar.Add("You do not have permission to perform this action.", Severity.Error);
                        _navigationManager.NavigateTo("/unauthorized");
                        break;

                    case System.Net.HttpStatusCode.InternalServerError:
                        _snackbar.Add("A critical server error occurred. Please contact IT support.", Severity.Error);
                        break;
                }
            }

            return response;
        }
    }
}
