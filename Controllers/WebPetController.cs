using InnoGotchi_backend.Models.DTOs;
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

        [Route("pet-overview")]
        public IActionResult GetPet(PetDto pet)
        {
            return View("PetOverview",pet);
        }

        public IActionResult CheckRadio(IFormCollection form)
        {
            PetDto pet = new PetDto();
            pet.Body = form["body"].ToString();
            pet.Nose = form["nose"].ToString();
            pet.Eyes = form["eye"].ToString();
            pet.Mouth = form["mouth"].ToString();
            return RedirectToAction("pet-overview","pet",pet);
        }
    }
}
