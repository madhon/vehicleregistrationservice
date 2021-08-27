﻿namespace VehicleRegistrationService.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using VehicleRegistrationService.Model;
    using VehicleRegistrationService.Repositories;

    [ApiController]
    [Route("[controller]")]
    public class VehicleInfoController : ControllerBase
    {
        private readonly ILogger<VehicleInfoController> _logger;
        private readonly IVehicleInfoRepository _vehicleInfoRepository;

        public VehicleInfoController(ILogger<VehicleInfoController> logger, IVehicleInfoRepository vehicleInfoRepository)
        {
            _logger = logger;
            _vehicleInfoRepository = vehicleInfoRepository;
        }

        [HttpGet("{licenseNumber}")]
        public ActionResult<VehicleInfo> GetVehicleInfo(string licenseNumber)
        {
            _logger.LogInformation($"Retrieving vehicle-info for licensenumber {licenseNumber}");
            VehicleInfo info = _vehicleInfoRepository.GetVehicleInfo(licenseNumber);
            return info;
        }
    }
}
