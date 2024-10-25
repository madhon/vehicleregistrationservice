namespace VehicleRegistrationService;

[JsonSourceGenerationOptions(
    PropertyNameCaseInsensitive = true,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    DefaultIgnoreCondition = JsonIgnoreCondition.Never,
    NumberHandling = JsonNumberHandling.AllowReadingFromString,
    GenerationMode = JsonSourceGenerationMode.Default)
]
[JsonSerializable(typeof(BuildInfoResponse))]
[JsonSerializable(typeof(LoginRequest))]
[JsonSerializable(typeof(LoginResponse))]
[JsonSerializable(typeof(VehicleInfo))]
[JsonSerializable(typeof(VehicleInfoRequest))]
internal sealed partial class AppJsonSerializerContext : JsonSerializerContext;