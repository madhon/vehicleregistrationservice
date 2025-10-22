namespace VehicleRegistrationService;

[JsonSourceGenerationOptions(
    PropertyNameCaseInsensitive = true,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    DefaultIgnoreCondition = JsonIgnoreCondition.Never,
    NumberHandling = JsonNumberHandling.AllowReadingFromString,
    GenerationMode = JsonSourceGenerationMode.Default),
]
[JsonSerializable(typeof(BuildInfoResponse))]
[JsonSerializable(typeof(LoginRequest))]
[JsonSerializable(typeof(LoginResponse))]
[JsonSerializable(typeof(VehicleInfo))]
[JsonSerializable(typeof(VehicleInfoRequest))]

[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(IEnumerable<KeyValuePair<string,string?>>))]
[JsonSerializable(typeof(EnvResponse))]

[JsonSerializable(typeof(ProblemHttpResult))]
[JsonSerializable(typeof(ValidationProblem))]
[JsonSerializable(typeof(HttpValidationProblemDetails))]
internal sealed partial class AppJsonSerializerContext : JsonSerializerContext;