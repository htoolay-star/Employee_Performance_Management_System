using EPMS.Client.Handlers;
using EPMS.Client.Services;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using System.Text.Json;

namespace EPMS.Client.Extensions;

public static class ServiceCollectionExtensions
{
    public static IHttpClientBuilder AddSecureRefitClient<T>(
        this IServiceCollection services,
        RefitSettings settings,
        Uri baseUri) where T : class
    {
        return services.AddRefitClient<T>(settings)
            .ConfigureHttpClient(c => c.BaseAddress = baseUri)
            .AddHttpMessageHandler<AuthorizationMessageHandler>()
            .AddHttpMessageHandler<GlobalApiExceptionHandler>();
    }

    public static IServiceCollection AddApiClients(
        this IServiceCollection services,
        JsonSerializerOptions jsonOptions,
        Uri baseUri)
    {
        var refitSettings = new RefitSettings
        {
            ContentSerializer = new SystemTextJsonContentSerializer(jsonOptions)
        };

        services.AddSecureRefitClient<IAuthApiClient>(refitSettings, baseUri);
        services.AddSecureRefitClient<IPositionApiClient>(refitSettings, baseUri);
        services.AddSecureRefitClient<ILevelApiClient>(refitSettings, baseUri);
        services.AddSecureRefitClient<IDepartmentApiClient>(refitSettings, baseUri);
        services.AddSecureRefitClient<ITeamApiClient>(refitSettings, baseUri);

        return services;
    }
}