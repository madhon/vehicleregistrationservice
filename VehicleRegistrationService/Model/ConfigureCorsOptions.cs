namespace VehicleRegistrationService.Model;

using AspNetCorsOptions = Microsoft.AspNetCore.Cors.Infrastructure.CorsOptions;

internal sealed class ConfigureCorsOptions(IOptions<CorsOptions> corsOptions)
    : IConfigureOptions<AspNetCorsOptions>
{
    public void Configure(AspNetCorsOptions options)
    {
        options.AddPolicy(CorsPolicyName.Default, policy =>
        {
            var origins = corsOptions.Value.AllowedOrigins
                .Where(static o => !string.IsNullOrWhiteSpace(o))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray();

            if (origins.Length == 0)
            {
                // Blocks cross-origin browsers.
                // Same-origin Scalar (/scalar/v1 → /api/...) and Bruno are unaffected.
                policy.SetIsOriginAllowed(_ => false);
                return;
            }

            policy
                .WithOrigins(origins)
                .WithMethods("GET", "POST", "OPTIONS")
                .WithHeaders("Authorization", "Content-Type")
                .WithExposedHeaders("Token-Expired");
        });
    }
}
