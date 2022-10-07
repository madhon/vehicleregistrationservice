namespace VehicleRegistrationService.Model
{
    using System.ComponentModel;

    public class VehicleInfoRequest
    {
        public VehicleInfoRequest()
        {
            // needed for fast endpoints
        }

        /// <summary>
        /// The license plate to request info about
        /// </summary>
        [DefaultValue("Test")]
        public string LicenseNumber { get; set; } = string.Empty;
    }
}
