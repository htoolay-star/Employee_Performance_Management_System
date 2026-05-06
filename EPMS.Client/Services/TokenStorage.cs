using Blazored.LocalStorage;
using System.Threading.Tasks;

namespace EPMS.Client.Services
{
    public class TokenStorage
    {
        private readonly ILocalStorageService _localStorageService;
        private const string AccessTokenKey = "accessToken";
        private const string RefreshTokenKey = "refreshToken";

        public TokenStorage(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        public async ValueTask SetTokensAsync(string accessToken, string refreshToken)
        {
            await _localStorageService.SetItemAsync(AccessTokenKey, accessToken);
            await _localStorageService.SetItemAsync(RefreshTokenKey, refreshToken);
        }

        public async ValueTask<string?> GetAccessTokenAsync()
        {
            return await _localStorageService.GetItemAsync<string>(AccessTokenKey);
        }

        public async ValueTask<string?> GetRefreshTokenAsync()
        {
            return await _localStorageService.GetItemAsync<string>(RefreshTokenKey);
        }

        public async ValueTask ClearTokensAsync()
        {
            await _localStorageService.RemoveItemAsync(AccessTokenKey);
            await _localStorageService.RemoveItemAsync(RefreshTokenKey);
        }
    }
}
