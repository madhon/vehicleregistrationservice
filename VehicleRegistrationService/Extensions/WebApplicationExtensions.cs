namespace VehicleRegistrationService;

using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
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
        });

        app.UseAuthentication();
        app.UseAuthorization();

        //app.UseResponseCompression();
        app.UseResponseCaching();

        app.MapHealthChecks("/health/startup");
        app.MapHealthChecks("/healthz", new HealthCheckOptions { Predicate = _ => false });
        app.MapHealthChecks("/ready", new HealthCheckOptions { Predicate = _ => false });

        app.MapEnvEndpoint();
        app.MapConfEndpoint();
        app.MapVehicleRegistrationApi();
    }
}