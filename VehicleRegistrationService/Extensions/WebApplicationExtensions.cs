namespace VehicleRegistrationService
{
    using Microsoft.AspNetCore.Diagnostics.HealthChecks;

    public static class WebApplicationExtensions
    {
        public static void ConfigureApplication(this WebApplication app)
        {
            app.UseForwardedHeaders();

            app.UseSerilogRequestLogging();


            app.UseResponseCaching();
            app.UseResponseCompression();

            app.UseCors("AllowAll");

            app.UseStaticFiles();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseFastEndpoints(c =>
            {
                c.Endpoints.RoutePrefix = "api";
                c.Endpoints.ShortNames = true;
                c.Versioning.Prefix = "v";
                c.Versioning.PrependToRoute = true;
                c.Versioning.DefaultVersion = 1;
            });


            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.MapHealthChecks("/health/startup");
            app.MapHealthChecks("/healthz", new HealthCheckOptions { Predicate = _ => false });
            app.MapHealthChecks("/ready", new HealthCheckOptions { Predicate = _ => false });
        }
    }
}
