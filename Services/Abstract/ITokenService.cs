using InnoGotchi_backend.Models.Enums;
using Microsoft.AspNetCore.Mvc;

namespace InnoGotchi_frontend.Services.Abstract
{
    public interface ITokenService
    {
        public void AddTokenToCookie(string token, HttpContext context, string tokenName, int exipierDays);
        public void RemoveTokenFromCookie(HttpContext context);
        public bool IsTokenValid(HttpContext context);
    }
}
