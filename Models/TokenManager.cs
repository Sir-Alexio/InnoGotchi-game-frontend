using InnoGotchi_frontend.Services;
using Microsoft.AspNetCore.Mvc;

using System.Text.Json;
namespace InnoGotchi_frontend.Models
{
    public class TokenManager:Controller,ITokenManager
    {
        public async Task<ActionResult<string>> GetTokenFromCookie(HttpContext context)
        {
            string? token = context.Request.Cookies["token"];
            
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("No token");
            }

            return token;
        }

        public void AddTokenToCookie(string token, HttpContext context)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(1)
            };
            //Error
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
