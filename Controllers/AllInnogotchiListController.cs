using Microsoft.AspNetCore.Mvc;
using InnoGotchi_backend.Controllers;
using InnoGotchi_backend.DataContext;
using InnoGotchi_backend.Models;

namespace InnoGotchi_frontend.Controllers
{
    public class AllInnogotchiListController : Controller
    {
        PetController petController = new PetController(new ApplicationContext());
        public IActionResult Index()
        {
            return View(petController.GetPets());
        }
    }
}
