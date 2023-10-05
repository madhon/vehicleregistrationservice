namespace VehicleRegistrationService
{
    using System.Text;
    using FastEndpoints.Security;
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

            services.AddFastEndpoints(o => o.SourceGeneratorDiscoveredTypes.AddRange(DiscoveredTypes.All));


            services.AddAuthorization();

            var jwtOpts = new JwtOptions();
            configuration.Bind(JwtOptions.Jwt, jwtOpts);
            services.AddSingleton(Options.Create(jwtOpts));

            services.Configure<JwtOptions>(
                configuration.GetSection("JWT"));
            services.AddScoped(cfg => cfg!.GetService<IOptions<JwtOptions>>()!.Value);

            var secret = jwtOpts.Secret;
            var key = Encoding.ASCII.GetBytes(secret);

            services.AddJWTBearerAuth(secret, JWTBearer.TokenSigningStyle.Symmetric, opts =>
            {
                opts.ValidIssuer = jwtOpts.ValidIssuer;
                opts.ValidAudience = jwtOpts.ValidAudience;
            });

            services.SwaggerDocument(o =>
            {
                o.MaxEndpointVersion = 1;
                o.EnableJWTBearerAuth = true;
                o.ShortSchemaNames = true;
                o.DocumentSettings = d =>
                {
                    d.DocumentName = "v1.0";
                    d.Title = "v1.0";
                    d.Version = "v1.0";
                };
            });

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
