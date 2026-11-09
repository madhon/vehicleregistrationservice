namespace VehicleRegistrationService.Extensions;

using OpenFeature;

internal static class FeatureManagementExtensions
{
    public static TBuilder AddFeatureManagementServices<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        builder.Services.AddOpenFeature(featureBuilder =>
        {
            featureBuilder.AddProvider(sp =>
                new ConfigurationFeatureProvider(sp.GetRequiredService<IConfiguration>()));
        });

        return builder;
    }
}
