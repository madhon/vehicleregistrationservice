namespace VehicleRegistrationService.Extensions;

using Microsoft.AspNetCore.Authentication.JwtBearer;

internal static  class AuthorisationExtensions
{
    public static IServiceCollection AddAuthorisationServices(this IServiceCollection services)
    {
        services.AddAuthorization();

        services.AddSingleton<IConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>();

        services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, null!);

        return services;
    }
}