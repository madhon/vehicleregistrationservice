namespace VehicleRegistrationService.Model;

using System.Runtime.InteropServices;

internal sealed record VehicleInfoRequest([DefaultParameterValue("Test"), Optional] string LicenseNumber);