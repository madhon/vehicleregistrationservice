namespace VehicleRegistrationService;

using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.ResponseCompression;

internal static class WebApplicationBuilderExtensions
{
    private static readonly string[] second = new[]
            {
                "application/json",
                "application/problem+json",
                "application/vnd.api+json",
            };

    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.TypeInfoResolverChain.Insert(
                0, AppJsonSerializerContext.Default);
        });

        services.Configure<ForwardedHeadersOptions>(opts =>
        {
            opts.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            opts.KnownIPNetworks.Clear();
            opts.KnownProxies.Clear();
        });

        services.AddAppOptions();

        services.AddValidators();

        services.AddSingleton<TimeProvider>(TimeProvider.System);
        services.AddScoped<IVehicleInfoRepository, InMemoryVehicleInfoRepository>();
        services.AddSingleton<SigningAudienceCertificate>();
        services.AddSingleton<SigningIssuerCertificate>();

        services.AddCors(options =>
        {
            options.AddPolicy(CorsPolicyName.AllowAll, corsBuilder =>
            {
                corsBuilder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        services.AddOutputCache(options =>
        {
            options.AddBasePolicy(policy => policy.Expire(TimeSpan.FromMinutes(10)));
        });

        services.AddResponseCaching();
        services.AddResponseCompression(opts =>
        {
            opts.EnableForHttps = true;
            opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(second);
            opts.Providers.Add<BrotliCompressionProvider>();
            opts.Providers.Add<GzipCompressionProvider>();
        });

        services.AddProblemDetails();

        services.AddFeatureManagementServices();
        services.AddOpenApiServices();
        services.AddAuthorisationServices();

        return services;
    }
}