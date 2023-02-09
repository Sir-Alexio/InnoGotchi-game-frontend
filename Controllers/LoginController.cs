using Microsoft.AspNetCore.Mvc;

namespace InnoGotchi_frontend.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
