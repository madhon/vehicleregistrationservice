namespace VehicleRegistrationService.Endpoints;

using System.Text;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

public static class LoginEndpoint
{
    public static IEndpointRouteBuilder MapLoginEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("api/v1/login", Results<Ok<LoginResponse>, UnauthorizedHttpResult>
                (LoginRequest req, ILoggerFactory loggerFactory, IOptions<JwtOptions> options) =>
                {
                    //var logger = loggerFactory.CreateLogger("LoginEndpointV2");
                    if (!req.UserName!.Equals("jon") && !req.Password!.Equals("Password1"))
                    {
                        return TypedResults.Unauthorized();
                    }

                    var now = DateTime.UtcNow;
                    var unixTimeSeconds = new DateTimeOffset(now).ToUnixTimeSeconds();
                    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.Secret));
                    var signInCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
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
                        SigningCredentials = signInCredentials
                    };

                    var handler = new JsonWebTokenHandler();
                    var token = handler.CreateToken(descriptor);
                    return TypedResults.Ok(new LoginResponse { Token = token, ExpiresAt = now.AddMinutes(10) });
                })
            .WithName("login")
            .WithDescription("Login to API")
            .WithTags("login")
            .Produces<LoginResponse>()
            .Produces<UnauthorizedHttpResult>()
            .AllowAnonymous();

        return builder;
    }
}