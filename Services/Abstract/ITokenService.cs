using InnoGotchi_backend.Models.Enums;
using Microsoft.AspNetCore.Mvc;

namespace InnoGotchi_frontend.Services.Abstract
{
    public interface ITokenService
    {
        public void AddTokenToCookie(string token, HttpContext context);
        public void RemoveTokenFromCookie(HttpContext context);
    }
}
