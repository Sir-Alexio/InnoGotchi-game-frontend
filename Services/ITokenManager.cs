using Microsoft.AspNetCore.Mvc;

namespace InnoGotchi_frontend.Services
{
    public interface ITokenManager
    {
        public Task<ActionResult<string>> GetTokenFromCookie(HttpContext context);
        public void AddTokenToCookie(string token, HttpContext context);
        public void RemoveTokenFromCookie(HttpContext context);

    }
}
