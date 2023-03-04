using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Net.Http.Headers;
using InnoGotchi_backend.Models.Dto;
using InnoGotchi_frontend.Services;
using FluentValidation.AspNetCore;

namespace InnoGotchi_frontend.Controllers
{
    [Route("farm")]
    public class FarmsOverviewController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IValidationManager _validationManager;

        public FarmsOverviewController(IHttpClientFactory httpClientFactory, IValidationManager validationManager)
        {
            _httpClient = httpClientFactory.CreateClient("Client");
            _validationManager = validationManager;
        }

        [Route("farm-info")]
        public async Task<IActionResult> CreateFarm(FarmDto farmDto)
        {
            if (!_validationManager.FarmValidator.Validation(farmDto, this.ModelState).Result)
            {
                return View("CreateFarm", farmDto);
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["token"]);

            JsonContent content = JsonContent.Create(farmDto);

            HttpResponseMessage response = await _httpClient.PostAsync($"api/farm",content);

            if (!response.IsSuccessStatusCode)
            {
                //no message found

                //this.ModelState.AddModelError(String.Empty,"This farm name is already exist!");
                FluentValidation.Results.ValidationResult validationResult = await _validationManager.FarmValidator.AddError(farmDto, "This farm name is already exist!", this.ModelState);
                //validationResult.AddToModelState()

                return View("CreateFarm", farmDto);
            }

            FarmDto? farm = JsonSerializer.Deserialize<FarmDto>(response.Content.ReadAsStringAsync().Result);

            return View("FarmInfo", farm);
        }
        [Route("get-logic")]
        public async Task<IActionResult> GetLogic()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["token"]);

            HttpResponseMessage response = await _httpClient.GetAsync($"api/farm");

            FarmDto? dto = JsonSerializer.Deserialize<FarmDto>(response.Content.ReadAsStringAsync().Result);

            if (string.IsNullOrEmpty(dto.FarmName))
            {
                return View("CreateFarm");
            }

            return View("FarmInfo",dto);
        }

        public IActionResult Index()
        {
            return View("CreateFarm");
        }
    }
}
