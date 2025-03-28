namespace VehicleRegistrationService;

using VehicleRegistrationService.Endpoints;

public static class VehicleRegistrationApi
{
    public static RouteGroupBuilder MapVehicleRegistrationApi(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/", () => Results.LocalRedirect("~/scalar/v1")).ExcludeFromDescription();

        var api = routes.NewVersionedApi("VehicleRegistrationService");
        var group = api.MapGroup("api/");

        group.MapLoginEndpoint();
        group.MapGetVehicleInfoEndpoint();

        return group;
    }
}