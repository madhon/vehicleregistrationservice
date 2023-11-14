namespace VehicleRegistrationService.Endpoints
{
    public static class GetVehicleEndpointV2
    {
        public static IEndpointRouteBuilder MapGetVehicleInfoEndpoint(this IEndpointRouteBuilder builder)
        {
            builder.MapGet("api/v1/vehicleinfo/{licenseNumber}",
                async Task<Results<Ok<VehicleInfo>, ProblemHttpResult, UnauthorizedHttpResult, BadRequest<string>>>
                (string licenseNumber,  ILoggerFactory loggerFactory, IVehicleInfoRepository vehicleInfoRepository) =>
            {

                var logger = loggerFactory.CreateLogger("GetVehicleEndpointV2");


                logger.LogInformation("Retrieving vehicle info for license number {licenseNumber}", licenseNumber);


                if (licenseNumber.Equals("K27JSD", StringComparison.OrdinalIgnoreCase))
                {
                    logger.LogError("Request for restricted license number {licenseNumber}", licenseNumber);
                    return TypedResults.BadRequest("Restricted License Plate");
                }

                var info = vehicleInfoRepository.GetVehicleInfo(licenseNumber);

                return TypedResults.Ok(info);


            })
            .WithName("vehicleinfo2")
            .WithDescription("Retrieves info about the specified vehicle")
            .WithTags("vehicleinfo2")
            .Produces<VehicleInfo>()
            .ProducesProblemDetails()
            .Produces<UnauthorizedHttpResult>();

            return builder;
        }


    }
}
