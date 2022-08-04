namespace VehicleRegistrationService.Controllers
{
    using System.Security.Claims;

    [ApiController, Authorize]
    [Route("[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public partial class VehicleInfoController : ControllerBase
    {
        private readonly ILogger<VehicleInfoController> logger;
        private readonly IVehicleInfoRepository vehicleInfoRepository;

        public VehicleInfoController(ILogger<VehicleInfoController> logger, IVehicleInfoRepository vehicleInfoRepository)
        {
            this.logger = logger;
            this.vehicleInfoRepository = vehicleInfoRepository;
        }

        [HttpGet("{licenseNumber}", Name = nameof(GetVehicleInfo))]
        [ProducesResponseType(typeof(VehicleInfo), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetVehicleInfo(string licenseNumber)
        {
            LogLicensePlateRetrieved(licenseNumber);

            var name = User.FindFirst(ClaimTypes.Name).Value;

            if (licenseNumber.Equals("K27JSD", StringComparison.OrdinalIgnoreCase))
            {
                LogRestrictedLicensePlateRequested("K27JSD");
                return BadRequest("Restricted License Plate");
            }

            var info = vehicleInfoRepository.GetVehicleInfo(licenseNumber);
            return Ok(info);
        }
        
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
