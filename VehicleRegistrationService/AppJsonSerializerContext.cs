namespace VehicleRegistrationService;

[JsonSourceGenerationOptions(defaults: JsonSerializerDefaults.Web, GenerationMode = JsonSourceGenerationMode.Default)]
[JsonSerializable(typeof(BuildInfoResponse))]
[JsonSerializable(typeof(LoginRequest))]
[JsonSerializable(typeof(LoginResponse))]
[JsonSerializable(typeof(VehicleInfo))]
[JsonSerializable(typeof(VehicleInfoRequest))]
internal sealed partial class AppJsonSerializerContext : JsonSerializerContext;