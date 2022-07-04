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

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Secret));
            var signInCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokenOptions = new JwtSecurityToken(
                issuer: options.ValidIssuer,
                audience: options.ValidAudience,
                claims: new List<Claim>(),
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: signInCredentials
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return Ok(new JwtTokenResponse { Token = tokenString });
        }

    }
}
