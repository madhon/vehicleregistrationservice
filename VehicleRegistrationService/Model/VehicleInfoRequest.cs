namespace VehicleRegistrationService.Model;

using System.Runtime.InteropServices;

public record VehicleInfoRequest([DefaultParameterValue("Test"), Optional] string LicenseNumber);