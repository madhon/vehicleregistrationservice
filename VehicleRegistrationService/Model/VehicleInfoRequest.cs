namespace VehicleRegistrationService.Model;

using System.ComponentModel;

public record VehicleInfoRequest([DefaultValue("Test")] string LicenseNumber);