namespace VehicleRegistrationService.Endpoints;

public static class EnvEndpoint
{
    public static IEndpointRouteBuilder MapEnvEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("env", (IWebHostEnvironment? hostEnvironment) =>
            {
                var thisEnv = new
                {
                    ApplicationName = hostEnvironment?.ApplicationName,
                    Environment = hostEnvironment?.EnvironmentName,
                };

                return Results.Ok(thisEnv);
            })
            .AllowAnonymous()
            .WithName("env")
            .WithDescription("Get Environment Info")
            .WithTags("env");

        return builder;
    }
}