namespace VehicleRegistrationService
{
    using Microsoft.AspNetCore.HttpOverrides;
    
    public static class WebApplicationBuilderExtensions
    {
        public static void RegisterServices(this WebApplicationBuilder builder)
        {
            var services = builder.Services;
            var configuration = builder.Configuration;

            services.Configure<ForwardedHeadersOptions>(opts =>
            {
                opts.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                opts.KnownNetworks.Clear();
                opts.KnownProxies.Clear();
            });


            services.AddScoped<IVehicleInfoRepository, InMemoryVehicleInfoRepository>();

            services.AddHealthChecks();

            //services.AddFastEndpoints(o =>
            //{
            //    o.SourceGeneratorDiscoveredTypes = DiscoveredTypes.All;
            //});

            services.AddAuthenticationJWTBearer(configuration["JWT:Secret"],
                configuration["JWT:ValidIssuer"],
                configuration["JWT:ValidAudience"]);

            services.AddSwaggerDoc(shortSchemaNames: true);

            services.Configure<JwtOptions>(
                configuration.GetSection("JWT"));
            services.AddScoped(cfg => cfg!.GetService<IOptions<JwtOptions>>()!.Value);

            //builder.Services.AddAuthentication(opt => {
            //        opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //        opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //        opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            //    })
            //    .AddJwtBearer(options =>
            //    {
            //        options.SaveToken = true;
            //        options.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"])),
            //            ValidateIssuer = true,
            //            ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
            //            ValidateAudience = true,
            //            ValidAudience = builder.Configuration["JWT:ValidAudience"],
            //            ValidateLifetime = true,
            //            ValidateIssuerSigningKey = true,
            //            RequireExpirationTime = true
            //        };
            //    });
        }

    }
}
