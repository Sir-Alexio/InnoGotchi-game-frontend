using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Net.Http.Headers;
using InnoGotchi_frontend.Services.Abstract;
using InnoGotchi_backend.Models.DTOs;

namespace InnoGotchi_frontend.Controllers
{
    [Route("innogotchi")]
    public class AllInnogotchiListController : Controller
    {
        private readonly HttpClient _client;
        private readonly ITokenService _tokenService;

        private static List<PetDto> _pets;

        public AllInnogotchiListController(IHttpClientFactory clientFactory, ITokenService tokenService)
        {
            _client = clientFactory.CreateClient("Client");
            _tokenService = tokenService;
        }

        [Route("innogotches")]
        public async Task<ActionResult> GetALlInnogotches()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["token"]);

            HttpResponseMessage response = await _client.GetAsync("api/pet/pets");

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest("UnAuthorized");
            }
            List<PetDto>? pets = JsonSerializer.Deserialize<List<PetDto>>(await response.Content.ReadAsStringAsync());

            pets = pets.OrderBy(x => x.HappyDaysCount).Reverse().ToList();

            _pets = pets;

            return View("InnogotchiList", pets);
        }

        [Route("sorted-innogotches")]
        public async Task<ActionResult> SortedInnogotches(string selectedItem)
        {
            List<PetDto> pets = new List<PetDto>();
            switch (selectedItem)
            {
                case "Happy days":
                    pets = _pets.OrderBy(x => x.HappyDaysCount).Reverse().ToList();
                    break;
                case "Age":
                    pets = _pets.OrderBy(x => x.AgeDate).ToList();
                    break;
                case "Hunger":
                    pets = _pets.OrderBy(x => x.LastHungerLevel).Reverse().ToList();
                    break;
                case "Thirsty":
                    pets = _pets.OrderBy(x => x.LastThirstyLevel).Reverse().ToList();
                    break;
                default:
                    break;
            }

            return View("InnogotchiList", pets);
        }



    }
}
