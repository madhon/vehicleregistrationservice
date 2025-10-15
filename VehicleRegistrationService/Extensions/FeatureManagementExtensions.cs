namespace VehicleRegistrationService.Extensions;

using Microsoft.FeatureManagement;
using OpenFeature;
using OpenFeature.Contrib.Providers.FeatureManagement;

internal static class FeatureManagementExtensions
{
    public static IServiceCollection AddFeatureManagementServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddFeatureManagement();

        services.AddOpenFeature(featureBuilder =>
        {
            featureBuilder
                .AddHostedFeatureLifecycle()
                .AddProvider(_ =>
                {
                    var openFeatureManagementProvider = new FeatureManagementProvider(configuration);
                    return openFeatureManagementProvider;
                });
        });

        return services;
    }
}