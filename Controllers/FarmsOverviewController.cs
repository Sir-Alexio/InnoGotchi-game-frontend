using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Net.Http.Headers;
using InnoGotchi_backend.Models.Dto;
using FluentValidation.AspNetCore;
using InnoGotchi_frontend.Services.Abstract;
using InnoGotchi_frontend.Models.Validators;
using InnoGotchi_backend.Models.Entity;

namespace InnoGotchi_frontend.Controllers
{
    [Route("farm")]
    public class FarmsOverviewController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenService _tokenService;

        public FarmsOverviewController(IHttpClientFactory httpClientFactory, ITokenService tokenService)
        {
            _httpClient = httpClientFactory.CreateClient("Client");
            _tokenService = tokenService;
        }

        [Route("farm-overview")]
        public IActionResult GetFarmOverviewPage()
        {
            return View("FarmOverviewPage");
        }

        [Route("farm-info")]
        public async Task<IActionResult> CreateFarm(FarmDto farmDto)
        {
            FarmValidator validator = new FarmValidator();

            if (!validator.Validate(farmDto).IsValid)
            {
                return View("CreateFarm", farmDto);
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["token"]);

            JsonContent content = JsonContent.Create(farmDto);

            HttpResponseMessage response = await _httpClient.PostAsync($"api/farm/new-farm",content);

            if (!response.IsSuccessStatusCode)
            {
                CustomExeption? errorMessage = JsonSerializer.Deserialize<CustomExeption>(response.Content.ReadAsStringAsync().Result);

                ViewBag.Message = errorMessage.Message;

                return View("CreateFarm", farmDto);
            }

            FarmDto? farm = JsonSerializer.Deserialize<FarmDto>(response.Content.ReadAsStringAsync().Result);

            return View("FarmInfo", farm);
        }

        [Route("my-own-farm")]
        public async Task<IActionResult> GetLogic()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["token"]);

            HttpResponseMessage response = await _httpClient.GetAsync($"api/farm/current-farm");

            FarmDto? dto = JsonSerializer.Deserialize<FarmDto>(response.Content.ReadAsStringAsync().Result);

            if (string.IsNullOrEmpty(dto.FarmName))
            {
                return View("CreateFarm");
            }

            return View("FarmInfo",dto);
        }

        [Route("farm-detail")]
        public IActionResult GetFarmDetailPage()
        {
            return RedirectToAction("all-users", "user");
        }
    }
}
