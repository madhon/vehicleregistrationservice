namespace VehicleRegistrationService
{
    using VehicleRegistrationService.Controllers;

    public static class LoggerExtensions
    {

        private static readonly Action<ILogger<VehicleInfoController>, string, Exception> _LogLicensePlateRetrieved =
            LoggerMessage.Define<string>(
                LogLevel.Information,
                new EventId(1, nameof(_LogLicensePlateRetrieved)),
                "Retrieving vehicle-info for license number {licenseNumber}"
            );

        public static void LogLicensePlateRetrieved(this ILogger<VehicleInfoController> logger, string registration)
        {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            _LogLicensePlateRetrieved(logger, registration, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        }

    }
}
