namespace VehicleRegistrationService;

using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

public static class WebApplicationBuilderExtensions
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

        services.AddResponseCaching();
        services.AddResponseCompression();

        services.AddHealthChecks();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(config =>
        {
            config.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Vehicle Registration API"
            });

            config.AddSecurityRequirement(new OpenApiSecurityRequirement {
            {
                new OpenApiSecurityScheme {
                    Reference = new OpenApiReference {
                        Type = ReferenceType.SecurityScheme,
                        Id = JwtBearerDefaults.AuthenticationScheme
                    }
                },
                Array.Empty<string>()
            }});
                
            config.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "Input your Bearer token to access this API",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                BearerFormat = "JWT",
            });
        });
            
        services.AddAuthorization();

        var jwtOpts = new JwtOptions();
        configuration.Bind(JwtOptions.SectionName, jwtOpts);
        services.AddSingleton(Options.Create(jwtOpts));

        services.Configure<JwtOptions>(
            configuration.GetSection(JwtOptions.SectionName));
        services.AddScoped(cfg => cfg!.GetService<IOptions<JwtOptions>>()!.Value);

        var secret = jwtOpts.Secret;
        var key = Encoding.ASCII.GetBytes(secret);

        builder.Services.AddAuthentication(opt => {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"])),
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:ValidAudience"],
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    RequireExpirationTime = true
                };
            });
    }

}