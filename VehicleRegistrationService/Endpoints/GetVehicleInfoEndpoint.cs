namespace VehicleRegistrationService.Endpoints
{
    public partial class GetVehicleInfoEndpoint : Endpoint<VehicleInfoRequest, VehicleInfo>
    {
        private readonly ILogger<GetVehicleInfoEndpoint> logger;
        private readonly IVehicleInfoRepository vehicleInfoRepository;

        public GetVehicleInfoEndpoint(ILogger<GetVehicleInfoEndpoint> logger, IVehicleInfoRepository vehicleInfoRepository)
        {
            this.logger = logger;
            this.vehicleInfoRepository = vehicleInfoRepository;
        }

        public override void Configure()
        {
            Get("/VehicleInfo/{LicenseNumber}");
            Summary(s =>
            {
                s.Summary = "Retrieve vehicle info";
                s.Description = "Retrieves info about the specified vehicle";
                s.ExampleRequest = new VehicleInfo()
                {
                    Brand = "BMW", 
                    Model = "X5", 
                    OwnerEmail = "Test@test.com", 
                    OwnerName = "Test",
                    VehicleId = "BMWX5"
                };
            });
        }

#pragma warning disable AsyncFixer01 // Unnecessary async/await usage
        public override async Task HandleAsync(VehicleInfoRequest req, CancellationToken ct)
        {
            LogLicensePlateRetrieved(req.LicenseNumber);

            if (req.LicenseNumber.Equals("K27JSD", StringComparison.OrdinalIgnoreCase))
            {
                LogRestrictedLicensePlateRequested("K27JSD");
                ThrowError("Restricted License Plate");
            }
            
            var info = vehicleInfoRepository.GetVehicleInfo(req.LicenseNumber);
            await SendAsync(info, cancellation: ct).ConfigureAwait(false);
        }
#pragma warning restore AsyncFixer01 // Unnecessary async/await usage


        [LoggerMessage(eventId: 1,
            level: LogLevel.Information,
            message: "Retrieving vehicle info for license number {registration}")]
        partial void LogLicensePlateRetrieved(string registration);

        [LoggerMessage(eventId: 2,
            level: LogLevel.Information,
            message: "Request for restricted license number {registration}")]
        partial void LogRestrictedLicensePlateRequested(string registration);
    }
}
