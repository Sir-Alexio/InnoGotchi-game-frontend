using InnoGotchi_backend.Models;
using InnoGotchi_backend.Models.DTOs;
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

        [Route("current-pet/{petName}")]
        public async Task<ActionResult> GetCurrentPetView(string petName)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["token"]);

            HttpResponseMessage response = await _httpClient.GetAsync($"api/pet/current-pet/{petName}");

            if (!response.IsSuccessStatusCode)
            {
                CustomExeption? errorMessage = JsonSerializer.Deserialize<CustomExeption>(response.Content.ReadAsStringAsync().Result);

                ViewBag.Message = errorMessage.Message;

                return View("GetPetListPage");
            }

            return View("CurrentPetOverview", JsonSerializer.Deserialize<PetDto>(response.Content.ReadAsStringAsync().Result));
        }

        [Route("feed-current-pet/{petName}")]
        public async Task<IActionResult> FeedCurrentPet(string petName)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["token"]);
            
            JsonContent content = JsonContent.Create(petName);

            HttpResponseMessage response = await _httpClient.PatchAsync("api/pet/feed-current-pet", content);

            if (!response.IsSuccessStatusCode)
            {
                CustomExeption? errorMessage = JsonSerializer.Deserialize<CustomExeption>(response.Content.ReadAsStringAsync().Result);

                ViewBag.Message = errorMessage.Message;

                return RedirectToAction("pet-list");
            }

            return RedirectToAction("pet-list");
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

        [Route("pet-list")]
        public async Task<IActionResult> GetPetListPage()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["token"]);

            HttpResponseMessage response = await _httpClient.GetAsync("api/pet/all-pets");

            if (!response.IsSuccessStatusCode)
            {
                CustomExeption? errorMessage = JsonSerializer.Deserialize<CustomExeption>(response.Content.ReadAsStringAsync().Result);

                ViewBag.Message = errorMessage.Message;

                return View("FarmInfo","farm");
            }

            List<PetDto>? pets = JsonSerializer.Deserialize<List<PetDto>>(response.Content.ReadAsStringAsync().Result);

            return View("GetPetListPage",pets);
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
        public async Task<IActionResult> CreateNewPet(PetDto dto)
        {
            PetValidator validator = new PetValidator();

            if (!validator.Validate(dto).IsValid)
            {
                return View("PetOverview", _pet);
            }

            _pet.PetName = dto.PetName;

            _pet.AgeDate = DateTime.Now;
            _pet.LastHungerLevel = DateTime.Now;
            _pet.LastThirstyLevel = DateTime.Now;

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["token"]);

            JsonContent content = JsonContent.Create(_pet);

            HttpResponseMessage response = await _httpClient.PostAsync($"api/pet/new-pet", content);

            if (!response.IsSuccessStatusCode)
            {
                CustomExeption? errorMessage = JsonSerializer.Deserialize<CustomExeption>(response.Content.ReadAsStringAsync().Result);

                ViewBag.Message = errorMessage.Message;

                return View("PetOverview", _pet);
            }

            return RedirectToAction("my-own-farm", "farm");
        }
    }
}
