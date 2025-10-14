namespace VehicleRegistrationService.Model;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

internal sealed class ConfigureJwtBearerOptions : IConfigureNamedOptions<JwtBearerOptions>
{
    private readonly SigningIssuerCertificate issuerCertificate;
    private readonly JwtOptions jwtOptions;

    public ConfigureJwtBearerOptions(SigningIssuerCertificate issuerCertificate, IOptions<JwtOptions> jwtOptions)
    {
        this.issuerCertificate = issuerCertificate;
        this.jwtOptions = jwtOptions.Value;
    }

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
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                {
                    context.Response.Headers.Append("Token-Expired", "true");
                }
                return Task.CompletedTask;
            },
        };
    }
}