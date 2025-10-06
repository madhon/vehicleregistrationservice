namespace VehicleRegistrationService;

using System.Diagnostics.CodeAnalysis;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.FeatureManagement;
using Microsoft.OpenApi.Models;

internal static class WebApplicationBuilderExtensions
{
    private static readonly string[] second = new[]
            {
                "application/json",
                "application/problem+json",
                "application/vnd.api+json",
            };

    [SuppressMessage("Design", "MA0051:Method is too long")]
    public static IHostApplicationBuilder RegisterServices(this IHostApplicationBuilder builder)
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


        services.AddScoped<IValidator<LoginRequest>, LoginRequestValidator>();
        services.AddScoped<IValidator<VehicleInfoRequest>, VehicleInfoRequestValidator>();
        services.AddScoped<IVehicleInfoRepository, InMemoryVehicleInfoRepository>();
        services.AddScoped<SigningAudienceCertificate>();
        services.AddScoped<SigningIssuerCertificate>();

        services.AddCors(options =>
        {
            options.AddPolicy(CorsPolicyName.AllowAll, corsBuilder =>
            {
                corsBuilder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        builder.Services.AddOutputCache(options =>
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

        builder.AddDefaultHealthChecks();
        builder.ConfigureOpenTelemetry();

        services.AddProblemDetails();

        services.AddApiVersioning(opt =>
        {
            opt.ReportApiVersions = true;
            opt.AssumeDefaultVersionWhenUnspecified = true;
            opt.DefaultApiVersion = new ApiVersion(1, 0);
            opt.ApiVersionReader = ApiVersionReader.Combine(new QueryStringApiVersionReader(), new UrlSegmentApiVersionReader());
        });

        services.AddEndpointsApiExplorer();
        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, _, _) =>
            {
                document.Info = new OpenApiInfo
                {
                    Title = "Vehicle Registration API",
                    Version = "1.0.0",
                    Description = "Vehicle Registration API",
                    Contact = new OpenApiContact
                    {
                        Name = "Vehicle Registration API Team",
                    },
                };

                document.Servers = [];

                return Task.CompletedTask;
            });
            options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
            options.CustomSchemaIds(x => x.FullName?.Replace("+", ".", StringComparison.OrdinalIgnoreCase));
        });

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

        builder.Services.AddFeatureManagement();
        
        return builder;
    }

    internal static OpenApiOptions CustomSchemaIds(this OpenApiOptions config,
        Func<Type, string?> typeSchemaTransformer,
        bool includeValueTypes = false)
    {
        return config.AddSchemaTransformer((schema, context, _) =>
        {
            // Skip value types and strings
            if (!includeValueTypes && 
                (context.JsonTypeInfo.Type.IsValueType || 
                 context.JsonTypeInfo.Type == typeof(String) || 
                 context.JsonTypeInfo.Type == typeof(string)))
            {
                return Task.CompletedTask;
            }

            // Skip if the schema ID is not already set because we don't want to decorate the schema multiple times
            if (schema.Annotations == null || !schema.Annotations.TryGetValue("x-schema-id", out var _))
            {
                return Task.CompletedTask;
            }

            // transform the typename based on the provided delegate
            var transformedTypeName = typeSchemaTransformer(context.JsonTypeInfo.Type);

            // Scalar - decorate the models section
            schema.Annotations["x-schema-id"] = transformedTypeName;

            // Swagger and Scalar specific:
            // for Scalar - decorate the endpoint section
            // for Swagger - decorate the endpoint and model sections
            schema.Title = transformedTypeName;

            return Task.CompletedTask;
        });
    }
}