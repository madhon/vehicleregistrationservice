namespace VehicleRegistrationService.Model;

using System.ComponentModel.DataAnnotations;

public class JwtOptions
{
    public const string SectionName = "JWT";

    [Required, MinLength(1)]
    public string ValidAudience { get; set; } = string.Empty;
    
    [Required, MinLength(1)]
    public string ValidIssuer { get; set; } = string.Empty;
    
    public string Secret { get; set; } = string.Empty;

    public string EcDsaPrivate { get; set; } = string.Empty;
    public string EcDsaPublic { get; set; } = string.Empty;
    
}