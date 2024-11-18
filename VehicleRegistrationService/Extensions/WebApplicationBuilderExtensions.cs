namespace VehicleRegistrationService;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.OpenApi.Models;

internal static class WebApplicationBuilderExtensions
{
    public static void RegisterServices(this WebApplicationBuilder builder)
    {
        var services = builder.Services;
        var configuration = builder.Configuration;

        builder.Services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.TypeInfoResolverChain.Insert(
                0, AppJsonSerializerContext.Default);
        });

        services.Configure<ForwardedHeadersOptions>(opts =>
        {
            opts.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            opts.KnownNetworks.Clear();
            opts.KnownProxies.Clear();
        });

        services.AddScoped<IVehicleInfoRepository, InMemoryVehicleInfoRepository>();

        services.AddCors(options =>
        {
            options.AddPolicy(CorsPolicyName.AllowAll, builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        builder.Services.AddOutputCache(options =>
        {
            options.AddBasePolicy(policy => policy.Expire(TimeSpan.FromMinutes(10)));
        });

        services.AddResponseCaching();
        services.AddResponseCompression();

        services.AddHealthChecks();

        services.AddProblemDetails();

        services.AddOpenApi();

        services.AddAuthorization();

        builder.Services
            .AddOptions<JwtOptions>()
            .Bind(builder.Configuration.GetSection(JwtOptions.SectionName));

        builder.Services
            .AddSingleton<IValidateOptions<JwtOptions>, JwtOptionsValidator>();

        services.AddSingleton<IConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>();

        services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, null!);
    }
}