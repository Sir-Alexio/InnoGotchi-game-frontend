using Microsoft.AspNetCore.Mvc;

namespace InnoGotchi_frontend.Controllers
{
    public class FarmsOverviewController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
