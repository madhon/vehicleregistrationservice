namespace VehicleRegistrationService.Extensions;

[JsonSerializable(typeof(AzureFeatureFlagPayload))]
internal sealed partial class FeatureFlagJsonContext : JsonSerializerContext
{
}
