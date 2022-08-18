using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ForwardedHeadersOptions>(opts =>
{
    opts.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    opts.KnownNetworks.Clear();
    opts.KnownProxies.Clear();
});


builder.Services.AddScoped<IVehicleInfoRepository, InMemoryVehicleInfoRepository>();

builder.Services.AddHealthChecks();

builder.Services.AddFastEndpoints(o =>
{
    o.SourceGeneratorDiscoveredTypes = DiscoveredTypes.All;
});

builder.Services.AddAuthenticationJWTBearer(builder.Configuration["JWT:Secret"], 
    builder.Configuration["JWT:ValidIssuer"], 
    builder.Configuration["JWT:ValidAudience"]);

builder.Services.AddSwaggerDoc(shortSchemaNames: true);

builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection("JWT"));


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


var app = builder.Build();


app.UseForwardedHeaders();

app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseOpenApi();
app.UseSwaggerUi3();

app.UseAuthentication();
app.UseAuthorization();

app.UseFastEndpoints(c =>
{
    c.ShortEndpointNames = true;
});

app.MapHealthChecks("/health/startup");
app.MapHealthChecks("/healthz", new HealthCheckOptions { Predicate = _ => false });
app.MapHealthChecks("/ready", new HealthCheckOptions { Predicate = _ => false });

app.Run();