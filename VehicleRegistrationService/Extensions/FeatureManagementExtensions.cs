namespace VehicleRegistrationService.Extensions;

using Microsoft.FeatureManagement;

internal static class FeatureManagementExtensions
{
    public static IServiceCollection AddFeatureManagementServices(this IServiceCollection services)
    {
        services.AddFeatureManagement();
        return services;
    }
}