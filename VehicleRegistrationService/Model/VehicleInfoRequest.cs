namespace VehicleRegistrationService.Model
{
    using System.ComponentModel;

    public class VehicleInfoRequest
    {
        /// <summary>
        /// The license plate to request info about
        /// </summary>
        [DefaultValue("Test")]
        public string LicenseNumber { get; set; } = string.Empty;
    }
}
