namespace VehicleRegistrationService.Endpoints;

internal static partial class GetVehicleEndpoint
{
    public static IEndpointRouteBuilder MapGetVehicleInfoEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("vehicleinfo/{licenseNumber}", HandleGetVehicleEndpoint)
            .WithName(nameof(HandleGetVehicleEndpoint))
            .WithDescription("Retrieves info about the specified vehicle")
            .WithTags("vehicleinfo")
            .Produces<VehicleInfo>()
            .ProducesProblem(statusCode: StatusCodes.Status400BadRequest)
            .Produces<UnauthorizedHttpResult>()
            .RequireAuthorization();

        return builder;
    }

    private static async Task<Results<Ok<VehicleInfo>, ProblemHttpResult, UnauthorizedHttpResult>>
        HandleGetVehicleEndpoint(string licenseNumber,  ILoggerFactory loggerFactory, IVehicleInfoRepository vehicleInfoRepository)
    {
        var logger = loggerFactory.CreateLogger("GetVehicleEndpointV2");

        LogRetrievingLicense(logger, licenseNumber);

        if (licenseNumber.Equals("K27JSD", StringComparison.OrdinalIgnoreCase))
        {
            LogRestrictedLicense(logger, licenseNumber);
            return TypedResults.Problem("Restricted License Plate", statusCode: StatusCodes.Status400BadRequest);
        }

        var info = await vehicleInfoRepository.GetVehicleInfo(licenseNumber);
        return TypedResults.Ok(info);
    }

    [LoggerMessage(
        EventId = 1000,
        Level = LogLevel.Information,
        Message = "Retrieving vehicle info for license number `{licenseNumber}`")]
    static partial void LogRetrievingLicense(ILogger logger, string licenseNumber);

    [LoggerMessage(
        EventId = 1001,
        Level = LogLevel.Information,
        Message = "Request for restricted license number `{licenseNumber}`")]
    static partial void LogRestrictedLicense(ILogger logger, string licenseNumber);
}