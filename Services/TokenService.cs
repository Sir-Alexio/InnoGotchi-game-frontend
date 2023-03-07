using InnoGotchi_backend.Models.Enums;
using InnoGotchi_frontend.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

using System.Text.Json;
namespace InnoGotchi_frontend.Services
{
    public class TokenService : ITokenService
    {
        public void AddTokenToCookie(string token, HttpContext context)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(1)
            };

            context.Response.Cookies.Append("token", token, cookieOptions);
        }

        public void RemoveTokenFromCookie(HttpContext context)
        {
            var options = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(-1)
            };

            context.Response.Cookies.Delete("token", options);
        }
    }
}
