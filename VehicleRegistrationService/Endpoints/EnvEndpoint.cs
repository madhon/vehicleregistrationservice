namespace VehicleRegistrationService.Endpoints;

internal static class EnvEndpoint
{
    public static IEndpointRouteBuilder MapEnvEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("env", (IWebHostEnvironment? hostEnvironment) =>
            {
                var response = new EnvResponse(hostEnvironment?.ApplicationName, hostEnvironment?.EnvironmentName);

                return Results.Ok(response);
            })
            .AllowAnonymous()
            .WithName("env")
            .WithDescription("Get Environment Info")
            .WithTags("env");

        return builder;
    }
}