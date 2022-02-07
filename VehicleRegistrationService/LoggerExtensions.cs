namespace VehicleRegistrationService
{
    using VehicleRegistrationService.Controllers;

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
    public static class LoggerExtensions
    {

        private static readonly Action<ILogger<VehicleInfoController>, string, Exception> _LogLicensePlateRetrieved =
            LoggerMessage.Define<string>(
                LogLevel.Information,
                new EventId(1, nameof(_LogLicensePlateRetrieved)),
                "Retrieving vehicle-info for license number {licenseNumber}"
            );

        private static readonly Action<ILogger<VehicleInfoController>, string, Exception> _LogRestrictedLicensePlateRequested =
            LoggerMessage.Define<string>(
                LogLevel.Information,
                new EventId(1, nameof(_LogRestrictedLicensePlateRequested)),
                "Request for restricted license number {licenseNumber}"
            );

        public static void LogLicensePlateRetrieved(this ILogger<VehicleInfoController> logger, string registration)
        {
            _LogLicensePlateRetrieved(logger, registration, null);
        }

        public static void LogRestrictedLicensePlateRequested(this ILogger<VehicleInfoController> logger, string registration)
        {
            _LogRestrictedLicensePlateRequested(logger, registration, null);
        }


    }
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
}
