using EPMS.Client.Handlers;
using EPMS.Client.Services.Auth;
using EPMS.Client.Services.Hr;
using EPMS.Client.Services.Info;
using EPMS.Client.Services.Performance;
using EPMS.Client.Services.Shared;
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

        // Auth
        services.AddSecureRefitClient<IAuthApiClient>(refitSettings, baseUri);
        services.AddSecureRefitClient<IPermissionApiClient>(refitSettings, baseUri);

        // Hr
        services.AddSecureRefitClient<IPositionApiClient>(refitSettings, baseUri);
        services.AddSecureRefitClient<ILevelApiClient>(refitSettings, baseUri);
        services.AddSecureRefitClient<IDepartmentApiClient>(refitSettings, baseUri);
        services.AddSecureRefitClient<ITeamApiClient>(refitSettings, baseUri);
        services.AddSecureRefitClient<ICategoryApiClient>(refitSettings, baseUri);

        // Performance
        services.AddSecureRefitClient<IAppraisalCycleApiClient>(refitSettings, baseUri);
        services.AddSecureRefitClient<IRatingScaleApiClient>(refitSettings, baseUri);
        services.AddSecureRefitClient<IKPIWeightPriorityApiClient>(refitSettings, baseUri);
        services.AddSecureRefitClient<IQuestionRatingScaleApiClient>(refitSettings, baseUri);
        services.AddSecureRefitClient<IEntityKPIApiClient>(refitSettings, baseUri);
        services.AddSecureRefitClient<IKPIMasterApiClient>(refitSettings, baseUri);
        services.AddSecureRefitClient<IEmployeeKPIApiClient>(refitSettings, baseUri);
        services.AddSecureRefitClient<IFormTemplateApiClient>(refitSettings, baseUri);
        services.AddSecureRefitClient<IFormQuestionApiClient>(refitSettings, baseUri);
        services.AddSecureRefitClient<IPositionFormTemplateApiClient>(refitSettings, baseUri);
        services.AddSecureRefitClient<IAppraisalApiClient>(refitSettings, baseUri);
        services.AddSecureRefitClient<IRecycleBinApiClient>(refitSettings, baseUri);
        services.AddSecureRefitClient<IEvaluationResponseApiClient>(refitSettings, baseUri);
        services.AddSecureRefitClient<IContinuousFeedbackApiClient>(refitSettings, baseUri);
        services.AddSecureRefitClient<IOneOnOneMeetingApiClient>(refitSettings, baseUri);

        // Info
        services.AddSecureRefitClient<IEmployeeProfileApiClient>(refitSettings, baseUri);
        services.AddSecureRefitClient<IEmployeeEmploymentApiClient>(refitSettings, baseUri);
        services.AddSecureRefitClient<IEmployeeContactApiClient>(refitSettings, baseUri);
        services.AddSecureRefitClient<IEmployeeFamilyInfoApiClient>(refitSettings, baseUri);
        services.AddSecureRefitClient<IEmployeePayrollInfoApiClient>(refitSettings, baseUri);

        return services;
    }
}