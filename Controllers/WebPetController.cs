using InnoGotchi_backend.Models;
using InnoGotchi_backend.Models.Dto;
using InnoGotchi_backend.Models.DTOs;
using InnoGotchi_frontend.Models;
using InnoGotchi_frontend.Models.Validators;
using InnoGotchi_frontend.Services;
using InnoGotchi_frontend.Services.Abstract;
using InnoGotchi_frontend.Validation;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace InnoGotchi_frontend.Controllers
{
    [Route("pet")]
    public class WebPetController : Controller
    {
        private readonly HttpClient _httpClient;
        private static PetDto _pet;

        public WebPetController(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient.CreateClient("Client");

            if(_pet == null)
                _pet = new PetDto(); 
        }

        [Route("constractor")]
        public IActionResult PetConstrator()
        {
            return View();
        }

        [Route("pet-overview")]
        public IActionResult GetPetOverview(PetDto pet)
        {
            return View("PetOverview",pet);
        }

        public IActionResult CheckRadio(IFormCollection form)
        {
            _pet.Body = form["body"].ToString();
            _pet.Nose = form["nose"].ToString();
            _pet.Eyes = form["eye"].ToString();
            _pet.Mouth = form["mouth"].ToString();

            return RedirectToAction("pet-overview","pet",_pet);
        }

        [Route("new-pet")]
        public async Task<IActionResult> OnPost(PetDto dto)
        {
            PetValidator validator = new PetValidator();

            if (!validator.Validate(dto).IsValid)
            {
                return View("PetOverview", _pet);
            }

            _pet.PetName = dto.PetName;
            
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["token"]);

            dto.HungerLevel = InnoGotchi_backend.Models.Enums.HungerLevel.Full;
            dto.ThirstyLevel = InnoGotchi_backend.Models.Enums.ThirstyLevel.Full;

            JsonContent content = JsonContent.Create(_pet);

            HttpResponseMessage response = await _httpClient.PostAsync($"api/pet/new-pet", content);

            if (!response.IsSuccessStatusCode)
            {
                CustomExeption? errorMessage = JsonSerializer.Deserialize<CustomExeption>(response.Content.ReadAsStringAsync().Result);

                ViewBag.Message = errorMessage.Message;

                return View("PetOverview", _pet);
            }

            return View("FarmInfo", "farm");
        }
    }
}
