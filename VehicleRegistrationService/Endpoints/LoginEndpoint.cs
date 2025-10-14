namespace VehicleRegistrationService.Endpoints;

using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using VehicleRegistrationService.Certificates;

internal static partial class LoginEndpoint
{
    private static readonly JsonWebTokenHandler TokenHandler = new();

    public static IEndpointRouteBuilder MapLoginEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("login", HandleLogin)
            .WithName("login")
            .WithDescription("Login to API")
            .WithTags("login")
            .Produces<LoginResponse>()
            .Produces<UnauthorizedHttpResult>()
            .Produces<ValidationProblem>()
            .AllowAnonymous();

        return builder;
    }

    private static Results<Ok<LoginResponse>, UnauthorizedHttpResult, ValidationProblem>
        HandleLogin(LoginRequest req,
            IValidator<LoginRequest> validator,
            ILoggerFactory loggerFactory,
            SigningAudienceCertificate signingAudienceCertificate,
            TimeProvider timeProvider,
            IOptions<JwtOptions> options)
    {
        var logger = loggerFactory.CreateLogger("LoginEndpointV2");

        var validationResult = validator.Validate(req);
        if (!validationResult.IsValid)
        {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        if (!(req.UserName.Equals("jon", StringComparison.OrdinalIgnoreCase) && req.Password.Equals("Password1", StringComparison.Ordinal)))
        {
            LogUserLoginFailed(logger, req.UserName);
            return TypedResults.Unauthorized();
        }

        LogUserLoginSuccess(logger, req.UserName);

        var now = timeProvider.GetUtcNow();
        var expiresAt = now.AddMinutes(120);
        var unixTimeSeconds = now.ToUnixTimeSeconds();

        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = options.Value.ValidIssuer,
            Audience = options.Value.ValidAudience,
            IssuedAt = now.DateTime,
            Expires = expiresAt.DateTime,
            Claims = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                { JwtRegisteredClaimNames.Iat, unixTimeSeconds.ToString(CultureInfo.InvariantCulture) },
                { JwtRegisteredClaimNames.Jti, Guid.CreateVersion7().ToString() },
                { ClaimTypes.Name, "jon" },
            },
            SigningCredentials = signingAudienceCertificate.GetAudienceSigningKey(),
        };

        var token = TokenHandler.CreateToken(descriptor);
        return TypedResults.Ok(new LoginResponse { Token = token, ExpiresAt = expiresAt.DateTime });
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