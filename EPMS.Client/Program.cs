using Blazored.LocalStorage;
using EPMS.Client;
using EPMS.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Load wwwroot/appsettings.json
builder.Configuration.AddJsonFile("wwwroot/appsettings.json", optional: true, reloadOnChange: true);

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<TokenStorage>();
builder.Services.AddTransient<AuthorizationMessageHandler>();

builder.Services.AddHttpClient<AuthApiClient>(client =>
{
    var config = builder.Configuration;
    var apiBaseUrl = config["ApiBaseUrl"] ?? builder.HostEnvironment.BaseAddress;
    client.BaseAddress = new Uri(apiBaseUrl);
}).AddHttpMessageHandler<AuthorizationMessageHandler>();

builder.Services.Configure<JsonSerializerOptions>(options =>
{
    options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
    options.PropertyNameCaseInsensitive = true;
});

builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthenticationStateProvider>();
builder.Services.AddAuthorizationCore();
builder.Services.AddMudServices();

await builder.Build().RunAsync();
