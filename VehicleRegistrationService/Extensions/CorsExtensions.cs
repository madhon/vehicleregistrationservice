namespace VehicleRegistrationService.Extensions;

internal static class CorsExtensions
{
    public static IServiceCollection AddAppCors(this IServiceCollection services)
    {
        services
            .AddOptions<CorsOptions>()
            .BindConfiguration(CorsOptions.SectionName);

        services.AddCors();
        services.AddSingleton<IConfigureOptions<Microsoft.AspNetCore.Cors.Infrastructure.CorsOptions>, ConfigureCorsOptions>();

        return services;
    }
}
