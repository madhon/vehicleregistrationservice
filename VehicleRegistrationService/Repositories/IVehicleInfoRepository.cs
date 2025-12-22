namespace VehicleRegistrationService.Repositories;

using VehicleRegistrationService.Model;

internal interface IVehicleInfoRepository
{
    ValueTask<VehicleInfo> GetVehicleInfo(string licenseNumber);
}