using InnoGotchi_frontend.Models;
using InnoGotchi_frontend.Services;
using InnoGotchi_frontend.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace InnoGotchi_frontend.Controllers
{
    public class WebPetController : Controller
    {
        private readonly IImageService _imageService;
        public WebPetController(IImageService imageService)
        {
            _imageService = imageService;
        }
        public IActionResult PetConstrator()
        {
            return View(_imageService.Images);
        }

        public IActionResult GetPet(ImageViewModel model)
        {
            var a = model;
            return View("FarmInfo","farm");
        }
    }
}
