using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace TestWeb.API.Helper
{
    public class TokenHelper
    {
        private readonly IConfiguration _configuration;

        public TokenHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GenerateAccessToken(string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var secret = _configuration.GetValue<string>("JWTToken:SecretKey");

            var secretKey = Convert.FromBase64String(secret);

            var claims = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, username)
            });

            var siginingCredential = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Issuer = _configuration.GetValue<string>("JWTToken:Issuer"),
                Audience = _configuration.GetValue<string>("JWTToken:Audience"),
                Expires = DateTime.Now.AddDays(10),
                SigningCredentials = siginingCredential
            };

            var securityToken  = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(securityToken);
            return accessToken;
        }

        public string GenerateRefreshToken()
        {
            var randonNumber = new byte[32];
           
            using (var rng = RandomNumberGenerator.Create()){

                rng.GetBytes(randonNumber);
                return Convert.ToBase64String(randonNumber);
            }
            
        }
    }
}
