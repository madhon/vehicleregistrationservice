namespace VehicleRegistrationService.Endpoints
{
    using System.IdentityModel.Tokens.Jwt;

    public class LoginEndpoint : Endpoint<Login>
    {
        private readonly JwtOptions jwtOptions;

        public LoginEndpoint(IOptions<JwtOptions> jwtOptions)
        {
            this.jwtOptions = jwtOptions.Value;
        }

        public override void Configure()
        {
            Post("/login");
            AllowAnonymous();
        }

#pragma warning disable AsyncFixer01 // Unnecessary async/await usage
        public override async Task HandleAsync(Login req, CancellationToken ct)
        {
            if (!req.UserName.Equals("jon") && !req.Password.Equals("Password1"))
            {
                ThrowError("The supplied credentials are invalid!");
            }

            var now = DateTime.UtcNow;

            var jwtToken = JWTBearer.CreateToken(
                signingKey: jwtOptions.Secret,
                expireAt: now.AddMinutes(10),
                issuer: jwtOptions.ValidIssuer,
                audience: jwtOptions.ValidAudience,
                claims: new[] { (ClaimTypes.Name, req.UserName), (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) }
            );

            await SendAsync(new { Token = jwtToken, ExpiresAt = now.AddMinutes(10) }, cancellation: ct).ConfigureAwait(false);
        }
#pragma warning restore AsyncFixer01 // Unnecessary async/await usage
    }
}
