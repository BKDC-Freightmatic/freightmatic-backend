using Freightmatic.Application.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Text;

namespace Freightmatic.Functions.Shared
{
    public class JwtValidation
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string _userId;

        public JwtValidation(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public bool ValidateJwtAccessToken(HttpRequest req)
        {
            // Check for Authorization header
            var authHeader = req.Headers.Authorization.FirstOrDefault();
            if (authHeader == null || !authHeader.StartsWith("Bearer "))
                throw new CustomException("Unauthorized", "Unauthorized", HttpStatusCode.Unauthorized);

            var token = authHeader.Substring("Bearer ".Length);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["AccessTokenKey"]);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                }, out SecurityToken validatedToken);


                // Cast the validated token to JwtSecurityToken to get claims
                var jwtToken = validatedToken as JwtSecurityToken;
                if (jwtToken != null)
                {
                    var claims = jwtToken.Claims; // Retrieve claims from the token

                    // Store claims in session
                    //_httpContextAccessor.HttpContext.Session.SetString("UserId", claims.First(x=> x.Type == "sub").Value);
                    _userId = claims.First(x => x.Type == "sub").Value;
                }
                return true; // Token is valid
            }
            catch
            {
                return false; // Token validation failed
            }
        }

        public bool ValidateJwtRefreshToken(HttpRequest req)
        {
            // Check for Authorization header
            var authHeader = req.Headers.Authorization.FirstOrDefault();
            if (authHeader == null || !authHeader.StartsWith("Bearer "))
                throw new CustomException("Unauthorized", "Unauthorized", HttpStatusCode.Unauthorized);

            var token = authHeader.Substring("Bearer ".Length);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["RefreshTokenKey"]);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                }, out SecurityToken validatedToken);

                return true; // Token is valid
            }
            catch
            {
                return false; // Token validation failed
            }
        }

        public string GetUserIdFromToken()
        {
            return _userId;
        }
    }
}
