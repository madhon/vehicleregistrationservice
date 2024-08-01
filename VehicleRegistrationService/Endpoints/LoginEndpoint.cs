namespace VehicleRegistrationService.Endpoints;

using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using VehicleRegistrationService.Certificates;

internal static partial class LoginEndpoint
{
    public static IEndpointRouteBuilder MapLoginEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("login", HandleLogin)
            .WithName("login")
            .WithDescription("Login to API")
            .WithTags("login")
            .Produces<LoginResponse>()
            .Produces<UnauthorizedHttpResult>()
            .AllowAnonymous();

        return builder;
    }

    private static Results<Ok<LoginResponse>, UnauthorizedHttpResult>
        HandleLogin(LoginRequest req, ILoggerFactory loggerFactory, IOptions<JwtOptions> options)
    {
        var logger = loggerFactory.CreateLogger("LoginEndpointV2");
        if (!(req.UserName!.Equals("jon", StringComparison.OrdinalIgnoreCase) && req.Password!.Equals("Password1", StringComparison.Ordinal)))
        {
            LogUserLoginFailed(logger, req.UserName);
            return TypedResults.Unauthorized();
        }

        LogUserLoginSuccess(logger, req.UserName);

        var now = DateTime.UtcNow;
        var unixTimeSeconds = new DateTimeOffset(now).ToUnixTimeSeconds();

        var signingAudienceCertificate = new SigningAudienceCertificate();

        //var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.Secret));
        //var signInCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = options.Value.ValidIssuer,
            Audience = options.Value.ValidAudience,
            IssuedAt = now,
            Expires = now.AddMinutes(10),
            Claims = new Dictionary<string, object>
            {
                { JwtRegisteredClaimNames.Iat, unixTimeSeconds.ToString() },
                { JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString() },
                { ClaimTypes.Name, "jon" }
            },
            SigningCredentials = signingAudienceCertificate.GetAudienceSigningKey()
        };

        var handler = new JsonWebTokenHandler();
        var token = handler.CreateToken(descriptor);
        return TypedResults.Ok(new LoginResponse { Token = token, ExpiresAt = now.AddMinutes(10) });
    }

    [LoggerMessage(
        EventId = 101,
        Level = LogLevel.Information,
        Message = "User Login Succeeded `{userName}`")]
    static partial void LogUserLoginSuccess(ILogger logger, string userName);

    [LoggerMessage(
        EventId = 102,
        Level = LogLevel.Information,
        Message = "User Login Failed `{userName}`")]
    static partial void LogUserLoginFailed(ILogger logger, string userName);
}