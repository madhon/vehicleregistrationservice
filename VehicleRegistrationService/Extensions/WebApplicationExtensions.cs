namespace VehicleRegistrationService;


using Scalar.AspNetCore;
using Serilog;
using VehicleRegistrationService.Endpoints;

internal static class WebApplicationExtensions
{
    public static void ConfigureApplication(this WebApplication app)
    {
        app.UseForwardedHeaders();
        app.UseAppSecurityHeaders();

        app.UseSerilogRequestLogging();

        app.UseCors(CorsPolicyName.Default);

        app.UseExceptionHandler();
        app.UseStatusCodePages();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.MapOpenApi().WithDocumentPerVersion().CacheOutput();
        app.MapScalarApiReference((opts) =>
        {
            opts.DefaultFonts = false;
            opts.WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
            opts.WithDotNetFlag();
            opts.WithNonce();
        });

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseResponseCompression();
        app.UseResponseCaching();

        app.MapDefaultEndpoints();

        app.MapEnvEndpoint();
        app.MapConfEndpoint();
        app.MapVehicleRegistrationApi();
    }
}
