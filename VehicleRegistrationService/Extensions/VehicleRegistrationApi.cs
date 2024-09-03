namespace VehicleRegistrationService;

using VehicleRegistrationService.Endpoints;

public static class VehicleRegistrationApi
{
    public static RouteGroupBuilder MapVehicleRegistrationApi(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/", () => TypedResults.LocalRedirect("~/docs"));

        var group = routes.MapGroup("api/v1/");

        group.MapLoginEndpoint();
        group.MapGetVehicleInfoEndpoint();

        return group;
    }
}