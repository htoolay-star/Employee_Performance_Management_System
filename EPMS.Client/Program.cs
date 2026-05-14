using Blazored.LocalStorage;
using EPMS.Client;
using EPMS.Client.Extensions;
using EPMS.Client.Handlers;
using EPMS.Client.Services.Auth;
using Mapster;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Configuration.AddJsonFile("wwwroot/appsettings.json", optional: true, reloadOnChange: true);

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<TokenStorage>();
builder.Services.AddTransient<AuthorizationMessageHandler>();
builder.Services.AddTransient<GlobalApiExceptionHandler>();

builder.Services.AddMapster();

var config = builder.Configuration;
var apiBaseUrl = config["ApiBaseUrl"] ?? builder.HostEnvironment.BaseAddress;
var baseUri = new Uri(apiBaseUrl);

var jsonOptions = new JsonSerializerOptions
{
    Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
    PropertyNameCaseInsensitive = true
};

builder.Services.AddApiClients(jsonOptions, baseUri);

builder.Services.AddHttpClient("RefreshClient", client =>
{
    client.BaseAddress = baseUri;
});

builder.Services.Configure<JsonSerializerOptions>(options =>
{
    options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
    options.PropertyNameCaseInsensitive = true;
});

builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthenticationStateProvider>();
builder.Services.AddScoped<LookupStateService>();
builder.Services.AddAuthorizationCore();
builder.Services.AddMudServices();

await builder.Build().RunAsync();
