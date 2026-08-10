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
            .AllowAnonymous()
            .RequireRateLimiting("login");

        return builder;
    }

    private static async Task<Results<Ok<LoginResponse>, UnauthorizedHttpResult, ValidationProblem>>
        HandleLogin(LoginRequest req,
            IValidator<LoginRequest> validator,
            ILoggerFactory loggerFactory,
            IFeatureClient featureClient,
            IJwtIdStore jwtIdStore,
            SigningAudienceCertificate signingAudienceCertificate,
            TimeProvider timeProvider,
            IOptions<JwtOptions> options,
            CancellationToken cancellationToken)
    {
        var logger = loggerFactory.CreateLogger("LoginEndpointV2");

        var validationResult = await validator.ValidateAsync(req, cancellationToken);
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
        var jti = Guid.CreateVersion7().ToString();

        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = options.Value.ValidIssuer,
            Audience = options.Value.ValidAudience,
            IssuedAt = now.DateTime,
            Expires = expiresAt.DateTime,
            Claims = new Dictionary<string, object>(capacity: 3, StringComparer.OrdinalIgnoreCase)
            {
                { JwtRegisteredClaimNames.Iat, unixTimeSeconds.ToString(CultureInfo.InvariantCulture) },
                { JwtRegisteredClaimNames.Jti, jti },
                { ClaimTypes.Name, "jon" },
            },
            SigningCredentials = signingAudienceCertificate.GetAudienceSigningKey(),
        };

        var token = TokenHandler.CreateToken(descriptor);

        if (await featureClient.GetBooleanValueAsync(
                FeatureFlags.JwtJtiReplayControl,
                defaultValue: false,
                cancellationToken: cancellationToken))
        {
            await jwtIdStore.RegisterAsync(jti, expiresAt, cancellationToken);
        }

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
