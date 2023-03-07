using InnoGotchi_frontend.Models;
using InnoGotchi_frontend.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace InnoGotchi_frontend.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITokenService _tokenManager;

        public HomeController(ILogger<HomeController> logger, ITokenService tokenManager)
        {
            _logger = logger;
            _tokenManager = tokenManager;
        }

        public IActionResult Index()
        {
            _tokenManager.RemoveTokenFromCookie(HttpContext);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}