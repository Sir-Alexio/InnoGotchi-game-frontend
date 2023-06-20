using Microsoft.AspNetCore.Mvc;

namespace InnoGotchi_frontend.Controllers
{
    [Route("farm-detail")]
    public class FarmDetailController : Controller
    {
        [Route("main-page")]
        public IActionResult GetFarmDetailPage()
        {
            return View("FarmDetailPage");
        }
    }
}
