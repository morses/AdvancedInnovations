using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace DiscordStats.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _config;
        public LoginController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet("GetToken")]
        [Authorize(AuthenticationSchemes = "Discord")]
        public object GetToken()
        {
            var userId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            

            string key = _config.GetValue<string>("Jwt:EncryptionKey");
            string issuer = _config.GetValue<string>("Jwt:Issuer");
            string audience = _config.GetValue<string>("Jwt:Audience");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var permClaims = new List<Claim>();
            permClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
           permClaims.Add(new Claim("discordId", userId));

            var token = new JwtSecurityToken(
                    issuer,
                    audience,
                    permClaims,
                    expires: DateTime.Now.AddDays(7),
                    signingCredentials: credentials
                );
            
            var jwt_token = new JwtSecurityTokenHandler().WriteToken(token);
            return new
            {
                ApiToken = jwt_token
            };
        }
    }
}
