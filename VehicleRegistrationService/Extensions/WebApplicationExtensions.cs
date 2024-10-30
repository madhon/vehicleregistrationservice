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

        app.UseSwagger(c =>
        {
            //c.RouteTemplate = "docs/{documentName}/openapi.json";
            c.RouteTemplate = "/openapi/{documentName}.json";
            c.PreSerializeFilters.Add((swagger, httpReq) => swagger.Servers = new List<OpenApiServer>
                { new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}{httpReq.PathBase.Value}" } });
        });

        app.UseSwaggerUI(c =>
        {
            c.RoutePrefix = "docs";
            c.SwaggerEndpoint("/openapi/v1.json", "Vehicle Registration API");
            c.DisplayRequestDuration();
            c.DefaultModelExpandDepth(-1);
        });

        app.MapScalarApiReference();

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