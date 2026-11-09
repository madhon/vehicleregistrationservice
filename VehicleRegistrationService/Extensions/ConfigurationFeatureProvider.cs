namespace VehicleRegistrationService.Extensions;

using OpenFeature.Constant;
using OpenFeature.Model;

internal sealed class ConfigurationFeatureProvider(IConfiguration configuration) : FeatureProvider
{
    public override Metadata GetMetadata() => new("AotConfigurationProvider");

    public override Task<ResolutionDetails<bool>> ResolveBooleanValueAsync(
        string flagKey,
        bool defaultValue,
        EvaluationContext? context = null,
        CancellationToken cancellationToken = default)
    {
        // 1. Check Azure App Configuration override (.appconfig.featureflag/<flagKey>)
        var azureFlagJson = configuration[$".appconfig.featureflag/{flagKey}"];
        if (!string.IsNullOrWhiteSpace(azureFlagJson))
        {
            var payload = JsonSerializer.Deserialize(
                azureFlagJson,
                FeatureFlagJsonContext.Default.AzureFeatureFlagPayload);

            if (payload is not null)
            {
                return Task.FromResult(new ResolutionDetails<bool>(
                    flagKey,
                    payload.Enabled,
                    reason: Reason.TargetingMatch));
            }
        }

        // 2. Read "FeatureManagement:<flagKey>" from appsettings.json
        var localFlagValue = configuration[$"FeatureManagement:{flagKey}"];
        if (bool.TryParse(localFlagValue, out var isEnabled))
        {
            return Task.FromResult(new ResolutionDetails<bool>(
                flagKey,
                isEnabled,
                reason: Reason.Static));
        }

        // 3. Fallback if missing
        return Task.FromResult(new ResolutionDetails<bool>(
            flagKey,
            defaultValue,
            reason: Reason.Default));
    }

    public override Task<ResolutionDetails<string>> ResolveStringValueAsync(
        string flagKey,
        string defaultValue,
        EvaluationContext? context = null,
        CancellationToken cancellationToken = default)
        => Task.FromResult(new ResolutionDetails<string>(
            flagKey,
            defaultValue,
            reason: Reason.Error,
            errorType: ErrorType.TypeMismatch));

    public override Task<ResolutionDetails<int>> ResolveIntegerValueAsync(
        string flagKey,
        int defaultValue,
        EvaluationContext? context = null,
        CancellationToken cancellationToken = default)
        => Task.FromResult(new ResolutionDetails<int>(
            flagKey,
            defaultValue,
            reason: Reason.Error,
            errorType: ErrorType.TypeMismatch));

    public override Task<ResolutionDetails<double>> ResolveDoubleValueAsync(
        string flagKey,
        double defaultValue,
        EvaluationContext? context = null,
        CancellationToken cancellationToken = default)
        => Task.FromResult(new ResolutionDetails<double>(
            flagKey,
            defaultValue,
            reason: Reason.Error,
            errorType: ErrorType.TypeMismatch));

    public override Task<ResolutionDetails<Value>> ResolveStructureValueAsync(
        string flagKey,
        Value defaultValue,
        EvaluationContext? context = null,
        CancellationToken cancellationToken = default)
        => Task.FromResult(new ResolutionDetails<Value>(
            flagKey,
            defaultValue,
            reason: Reason.Error,
            errorType: ErrorType.TypeMismatch));
}
