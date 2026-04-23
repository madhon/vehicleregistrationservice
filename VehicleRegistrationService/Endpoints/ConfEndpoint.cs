namespace VehicleRegistrationService.Endpoints;

using OpenFeature;

internal static class ConfEndpoint
{
    public static IEndpointRouteBuilder MapConfEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("conf", HandleConfig )
            .Produces<UnauthorizedHttpResult>()
            .RequireAuthorization()
            .WithName("conf")
            .WithDescription("Get Config Info")
            .WithTags("conf");

        return builder;
    }

    private static async Task<Results<Ok<IEnumerable<KeyValuePair<string, string?>>>, BadRequest>> HandleConfig(
        IConfiguration config,
        IFeatureClient featureClient)
    {
        ArgumentNullException.ThrowIfNull(config);

        if (await featureClient.GetBooleanValueAsync(FeatureFlags.DisableConfEndpoint, defaultValue: false))
        {
            return TypedResults.BadRequest();
        }

        var configKv = config.AsEnumerable();
        return TypedResults.Ok(configKv);

    }
}