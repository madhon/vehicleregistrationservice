namespace VehicleRegistrationService.Controllers
{
    using System.Net.Mime;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using VehicleRegistrationService.Model;
    using VehicleRegistrationService.Repositories;

    [ApiController]
    [Route("[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class VehicleInfoController : ControllerBase
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
        public ActionResult<VehicleInfo> GetVehicleInfo(string licenseNumber)
        {
            logger.LogLicensePlateRetrieved(licenseNumber);

            if (licenseNumber.Equals("K27JSD", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("Restricted License Plate");
            }

            var info = vehicleInfoRepository.GetVehicleInfo(licenseNumber);
            return info;
        }
    }
}
