using EPMS.Shared.DTOs.Auth;
using EPMS.Shared.DTOs.Common;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EPMS.Client.Services
{
    public class AuthApiClient
    {
        private readonly HttpClient _httpClient;

        public AuthApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<SuccessResponse<AuthResponse>> Login(LoginRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", request);
            response.EnsureSuccessStatusCode();

            var options = new JsonSerializerOptions
            {
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
                PropertyNameCaseInsensitive = true
            };

            return await response.Content.ReadFromJsonAsync<SuccessResponse<AuthResponse>>(options) ?? new SuccessResponse<AuthResponse>();
        }
    }
}
