namespace VehicleRegistrationService.Services;

internal interface IJwtIdStore
{
    ValueTask RegisterAsync(string jti, DateTimeOffset expiresAt, CancellationToken cancellationToken = default);
    ValueTask<bool> IsActiveAsync(string jti, CancellationToken cancellationToken = default);
}
