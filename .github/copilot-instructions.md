# Vehicle Registration Service - AI Coding Agent Instructions

## Architecture Overview

This is a modern .NET 9 ASP.NET Core minimal API service using JWT authentication for vehicle registration lookups. The service follows a clean, modular architecture with strict coding standards.

### Key Architectural Patterns

- **Minimal APIs with Extension Methods**: All endpoints are defined in separate classes under `Endpoints/` and registered via extension methods (e.g., `MapLoginEndpoint()`, `MapGetVehicleInfoEndpoint()`)
- **Service Registration Pattern**: `WebApplicationBuilderExtensions.RegisterServices()` handles all DI container setup
- **Application Configuration Pattern**: `WebApplicationExtensions.ConfigureApplication()` handles middleware pipeline setup
- **Repository Pattern**: Simple interface-based repositories in `Repositories/` (currently in-memory implementation)

### Critical Code Patterns

#### Endpoint Definition Pattern
```csharp
// Endpoints return strongly-typed Results<> unions
private static Results<Ok<VehicleInfo>, ProblemHttpResult, UnauthorizedHttpResult>
    HandleGetVehicleEndpoint(string licenseNumber, ...)

// Always use extension methods for registration
public static IEndpointRouteBuilder MapLoginEndpoint(this IEndpointRouteBuilder builder)
```

#### JSON Serialization
- **Native AOT Ready**: Uses `AppJsonSerializerContext` with source generators for all JSON serialization
- **All DTOs must be registered** in `AppJsonSerializerContext.cs` with `[JsonSerializable(typeof(...))]`

#### Validation Pattern
- **FluentValidation**: All request DTOs have corresponding validators in `Model/Validators/`
- **Always validate in endpoints**: Check `validationResult.IsValid` and return `TypedResults.ValidationProblem()`

#### Logging Pattern
- **Structured Logging**: Uses Serilog with structured logging via source generators
- **LoggerMessage Pattern**: Define logging methods with `[LoggerMessage]` attributes for performance

## Development Standards

### Banned Symbols
The `BannedSymbols.txt` file enforces strict coding standards:
- **NO Task.Result, Task.Wait** - Always use `await`
- **NO Autofac, AutoMapper, Newtonsoft.Json** - Use built-in alternatives
- **String Comparisons**: Use `StringComparison.Ordinal` not `InvariantCulture`
- **DateTime**: Use `.UtcNow` not `.Now`
- **Numeric Conversion**: Use `TryParse()` not `Convert.To*()`

### Global Usings
All common namespaces are in `GlobalUsings.cs` - avoid redundant using statements in individual files.

### Project Configuration
- **.NET 9** with latest C# language features
- **Native AOT compatible** - avoid reflection-heavy patterns
- **Strict Analyzers**: Roslynator, Meziantou, warnings as errors
- **OpenTelemetry**: Built-in metrics, tracing, and health checks

## Authentication & Security

- **JWT with RSA256**: Uses RSA key pairs (`private_key.xml`, `public_key.xml`) not HMAC
- **Hardcoded Test Credentials**: `jon/Password1` (demo service)
- **Certificate Classes**: `SigningAudienceCertificate` and `SigningIssuerCertificate` handle key management

## Testing & Local Development

### Running the Service
```bash
dotnet run --project VehicleRegistrationService
# Launches on http://localhost:57123
# API docs at /scalar/v1
```

### Test Endpoints
- **Login**: POST `/api/login` with `{"userName": "jon", "password": "Password1"}`
- **Vehicle Info**: GET `/api/vehicleinfo/{licenseNumber}` (requires JWT token)
- **Special License**: `K27JSD` returns restricted error, `K27ASD` returns Toyota CHR

### Docker Deployment
- Uses multi-service Docker Compose with Envoy proxy
- Traefik configuration commented out but available
- Health checks at `/healthz` and `/alive`

## Data & External Dependencies

- **In-Memory Repository**: Generates random vehicle/owner data using embedded `RandomNameGeneratorLibrary`
- **No Database**: Current implementation is stateless with mock data
- **Thread.Sleep Simulation**: Repository simulates network latency (5-200ms)

## API Documentation

- **Scalar UI**: Modern OpenAPI documentation at `/scalar/v1`
- **Versioned API**: Uses `Asp.Versioning` with query string and URL segment readers
- **Custom Schema IDs**: OpenAPI schemas use full type names with dot notation

When adding new features, follow these patterns strictly. Always add new DTOs to `AppJsonSerializerContext`, create validators, use source-generated logging, and maintain the extension method registration pattern.