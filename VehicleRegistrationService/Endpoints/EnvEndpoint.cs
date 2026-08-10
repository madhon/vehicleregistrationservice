namespace VehicleRegistrationService.Endpoints;

internal static class EnvEndpoint
{
    public static IEndpointRouteBuilder MapEnvEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("env", HandleEnv)
            .Produces<UnauthorizedHttpResult>()
            .Produces<BadRequest>()
            .RequireAuthorization()
            .WithName("env")
            .WithDescription("Get Environment Info")
            .WithTags("env");

        return builder;
    }

    private static async Task<Results<Ok<EnvResponse>, BadRequest>> HandleEnv(
        IWebHostEnvironment? hostEnvironment, IFeatureClient featureClient)
    {
        if (await featureClient.GetBooleanValueAsync(FeatureFlags.DisableEnvEndpoint, defaultValue: false))
        {
            return TypedResults.BadRequest();
        }

        var response = new EnvResponse(hostEnvironment?.ApplicationName, hostEnvironment?.EnvironmentName);

        return TypedResults.Ok(response);
    }
}
