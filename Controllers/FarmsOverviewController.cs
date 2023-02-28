using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Net.Http.Headers;
using InnoGotchi_backend.Models.Dto;

namespace InnoGotchi_frontend.Controllers
{
    [Route("farm")]
    public class FarmsOverviewController : Controller
    {
        private readonly HttpClient _httpClient;

        public FarmsOverviewController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("Client");
        }

        [Route("farm-info")]
        public async Task<IActionResult> CreateFarm(FarmDto farmDto)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["token"]);

            JsonContent content = JsonContent.Create(farmDto);

            HttpResponseMessage response = await _httpClient.PostAsync($"api/farm",content);

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest("Error 404");
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
