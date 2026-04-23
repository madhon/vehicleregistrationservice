namespace VehicleRegistrationService.Extensions;

using Microsoft.FeatureManagement;
using OpenFeature;
using OpenFeature.Contrib.Providers.FeatureManagement;

internal static class FeatureManagementExtensions
{
    public static TBuilder AddFeatureManagementServices<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        builder.Services.AddFeatureManagement();

        builder.Services.AddOpenFeature(featureBuilder =>
        {
            featureBuilder
                //.AddHostedFeatureLifecycle()
                .AddProvider(_ =>
                {
                    var openFeatureManagementProvider = new FeatureManagementProvider(builder.Configuration);
                    return openFeatureManagementProvider;
                });
        });

        return builder;
    }
}