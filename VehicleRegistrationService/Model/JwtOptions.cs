namespace VehicleRegistrationService.Model;

using System.ComponentModel.DataAnnotations;

internal sealed class JwtOptions
{
    public const string SectionName = "JWT";

    [Required, StringLength(1000, MinimumLength = 1)]
    public string ValidAudience { get; set; } = string.Empty;

    [Required, StringLength(1000, MinimumLength = 1)]
    public string ValidIssuer { get; set; } = string.Empty;

    public string EcDsaPrivate { get; set; } = string.Empty;
    public string EcDsaPublic { get; set; } = string.Empty;
}
