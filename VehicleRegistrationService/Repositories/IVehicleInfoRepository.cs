namespace VehicleRegistrationService.Repositories
{
    using VehicleRegistrationService.Model;

    public interface IVehicleInfoRepository
    {
        VehicleInfo GetVehicleInfo(string licenseNumber);
    }
}
