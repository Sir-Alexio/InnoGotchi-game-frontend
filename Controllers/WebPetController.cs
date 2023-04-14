using InnoGotchi_frontend.Models;
using InnoGotchi_frontend.Services;
using InnoGotchi_frontend.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace InnoGotchi_frontend.Controllers
{
    [Route("pet")]
    public class WebPetController : Controller
    {
        private readonly IImageService _imageService;

        public WebPetController(IImageService imageService)
        {
            _imageService = imageService;
            
        }
        [Route("constractor")]
        public IActionResult PetConstrator()
        {
            return View();
        }

        [Route("get-pet")]
        public IActionResult GetPet()
        {
            return View("FarmInfo","farm");
        }

        public string CheckRadio(IFormCollection form)
        {
            return form["body"].ToString() + "   " + form["eye"].ToString();
        }
    }
}
