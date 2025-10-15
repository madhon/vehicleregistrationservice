namespace VehicleRegistrationService;


using Scalar.AspNetCore;
using Serilog;
using VehicleRegistrationService.Endpoints;

internal static class WebApplicationExtensions
{
    public static void ConfigureApplication(this WebApplication app)
    {
        app.UseForwardedHeaders();

        app.UseSerilogRequestLogging();

        app.UseCors(CorsPolicyName.AllowAll);

        app.UseExceptionHandler();
        app.UseStatusCodePages();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.MapOpenApi().CacheOutput();
        app.MapScalarApiReference(opts =>
        {
            opts.DefaultFonts = false;
            opts.WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
            opts.WithDotNetFlag();
        });

        app.UseAuthentication();
        app.UseAuthorization();

        //app.UseResponseCompression();
        app.UseResponseCaching();

        app.MapDefaultEndpoints();

        app.MapEnvEndpoint();
        app.MapConfEndpoint();
        app.MapVehicleRegistrationApi();
    }
}