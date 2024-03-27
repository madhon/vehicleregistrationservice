namespace VehicleRegistrationService.Repositories;

using VehicleRegistrationService.Model;

internal interface IVehicleInfoRepository
{
    VehicleInfo GetVehicleInfo(string licenseNumber);
}