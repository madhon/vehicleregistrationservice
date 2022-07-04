namespace VehicleRegistrationService.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using AppBuildInfo = BuildInfo;

    [ApiController, AllowAnonymous]
    public class BuildInfoController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet("build")]
        public AppBuildInfo GetBuild() => AppVersionInfo.GetBuildInfo();
    }
}
