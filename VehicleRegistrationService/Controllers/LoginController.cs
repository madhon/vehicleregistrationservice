namespace VehicleRegistrationService.Controllers
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Net.Mime;
    using System.Security.Claims;
    using System.Text;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;
    using VehicleRegistrationService.Model;

    [ApiController, AllowAnonymous]
    [Route("[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class LoginController : ControllerBase
    {
        private readonly JwtOptions options;

        public LoginController(IOptions<JwtOptions> options)
        {
            this.options = options.Value;
        }

        [HttpPost("")]
        public IActionResult Login([FromBody] Login user)
        {
            if (!user.UserName.Equals("jon") && !user.Password.Equals("Password1"))
            {
                return Unauthorized();
            }

            var now = DateTime.UtcNow;

            //var key = ECDsa.Create(ECCurve.NamedCurves.nistP256);
            //var handler = new JsonWebTokenHandler()
            
            //var token = handler.CreateToken(new SecurityTokenDescriptor
            //{
            //    Issuer = options.ValidIssuer,
            //    Audience = options.ValidAudience,
            //    NotBefore = now,
            //    Expires = now.AddMinutes(10),
            //    IssuedAt = now,
            //    Claims = new Dictionary<string, object>(),
            //    SigningCredentials = new SigningCredentials(new ECDsaSecurityKey(key), "ES256")
            //});

            var unixTimeSeconds = new DateTimeOffset(now).ToUnixTimeSeconds();

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options!.Secret));
            var signInCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokenOptions = new JwtSecurityToken(
                issuer: options.ValidIssuer,
                audience: options.ValidAudience,
                claims: new List<Claim>()
                {
                    new Claim(JwtRegisteredClaimNames.Iat, unixTimeSeconds.ToString(), ClaimValueTypes.Integer64),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Name, "jon")
                },
                notBefore: now,
                expires: now.AddMinutes(10),
                signingCredentials: signInCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return Ok(new { Token = tokenString, ExpiresAt = now.AddMinutes(10) });
        }
    }
}
