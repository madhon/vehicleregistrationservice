namespace VehicleRegistrationService.Model;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

internal sealed class ConfigureJwtBearerOptions(
    SigningIssuerCertificate issuerCertificate,
    IJwtIdStore jwtIdStore,
    IOptions<JwtOptions> jwtOptions) : IConfigureNamedOptions<JwtBearerOptions>
{
    private readonly JwtOptions jwtOptions = jwtOptions.Value;

    public void Configure(JwtBearerOptions options)
    {
        Configure(string.Empty, options);
    }

    public void Configure(string? name, JwtBearerOptions options)
    {
        if (!string.Equals(name, JwtBearerDefaults.AuthenticationScheme, StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtOptions.ValidIssuer,
            ValidateAudience = true,
            ValidAudience = jwtOptions.ValidAudience,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = issuerCertificate.GetIssuerSigningKey(),
            ClockSkew = TimeSpan.FromSeconds(15),
            RequireExpirationTime = true,
            RequireSignedTokens = true,
            ValidAlgorithms = [SecurityAlgorithms.RsaSha256],
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                if (context.Exception is SecurityTokenExpiredException)
                {
                    context.Response.Headers.Append("Token-Expired", "true");
                }
                return Task.CompletedTask;
            },
            OnTokenValidated = async context =>
            {
                var featureClient = context.HttpContext.RequestServices
                    .GetRequiredService<IFeatureClient>();

                if (!await featureClient.GetBooleanValueAsync(
                        FeatureFlags.JwtJtiReplayControl,
                        defaultValue: false,
                        cancellationToken: context.HttpContext.RequestAborted))
                {
                    return;
                }

                var jti = context.Principal?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value
                          ?? context.Principal?.FindFirst("jti")?.Value;

                if (string.IsNullOrEmpty(jti)
                    || !await jwtIdStore.IsActiveAsync(jti, context.HttpContext.RequestAborted))
                {
                    context.Fail("Token jti is missing, unknown, or revoked.");
                }
            },
        };
    }
}
