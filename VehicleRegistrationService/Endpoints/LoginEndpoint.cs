namespace VehicleRegistrationService.Endpoints
{
    using FastEndpoints;
    using FastEndpoints.Security;
    using Microsoft.Extensions.Options;

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
                claims: new[] { ("Username", req.UserName) }
            );

            await SendAsync(new { Token = jwtToken, ExpiresAt = now.AddMinutes(10) }, cancellation: ct).ConfigureAwait(false);
        }
    }
}
