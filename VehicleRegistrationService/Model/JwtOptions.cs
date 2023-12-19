namespace VehicleRegistrationService.Model;

public class JwtOptions
{
    public const string SectionName = "JWT";

    public string ValidAudience { get; set; } = string.Empty;
    public string ValidIssuer { get; set; } = string.Empty;
    public string Secret { get; set; } = string.Empty;
}