using InnoGotchi_backend.Models.Entity;
using InnoGotchi_backend.Models.Enums;
using InnoGotchi_frontend.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
namespace InnoGotchi_frontend.Services
{
    public class TokenService : ITokenService
    {
        public void AddTokenToCookie(string token, HttpContext context,string tokenName,int exipierDays)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(exipierDays)
            };

            context.Response.Cookies.Append(tokenName, token, cookieOptions);
        }

        public void RemoveTokenFromCookie(HttpContext context)
        {
            var options = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(-1)
            };

            context.Response.Cookies.Delete("token", options);
        }

        public bool IsTokenValid(HttpContext context)
        {
            string? token = context.Request.Cookies["token"];

            if (string.IsNullOrWhiteSpace(token)) { throw new CustomExeption("no token"); }

            var tokenHandler = new JwtSecurityTokenHandler();

            // Read the JWT token
            var jwtToken = tokenHandler.ReadJwtToken(token);

            // Get the token's expiration date/time
            DateTime expirationDateTime = jwtToken.ValidTo;

            // Check if the token has expired
            return expirationDateTime < DateTime.UtcNow;
        }
    }
}
