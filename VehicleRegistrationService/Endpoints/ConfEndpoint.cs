namespace VehicleRegistrationService.Endpoints
{
    public static class ConfEndpoint
    {
        public static IEndpointRouteBuilder MapConfEndpoint(this IEndpointRouteBuilder builder)
        {
            builder.MapGet("conf", Results<Ok<IEnumerable<KeyValuePair<string, string?>>>, BadRequest> (IConfiguration? config) =>
            {
                var configKv = config.AsEnumerable();
                return TypedResults.Ok(configKv);
            })
            .AllowAnonymous()
            .WithName("conf")
            .WithDescription("Get Config Info")
            .WithTags("conf");

            return builder;
        }
    }
}