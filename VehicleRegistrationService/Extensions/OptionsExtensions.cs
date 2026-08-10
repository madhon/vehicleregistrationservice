namespace VehicleRegistrationService.Extensions;

internal static class OptionsExtensions
{
    public static IServiceCollection AddAppOptions(this IServiceCollection services)
    {
        services
            .AddOptions<JwtOptions>()
            .BindConfiguration(JwtOptions.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services
            .AddSingleton<IValidateOptions<JwtOptions>, JwtOptionsValidator>();

        return services;
    }

}
