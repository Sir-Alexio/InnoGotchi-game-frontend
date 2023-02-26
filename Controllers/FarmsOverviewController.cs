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
        private static UserDto _user;

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

            return RedirectToAction("personal-info","account");
        }
        public IActionResult Index()
        {
            return View("CreateFarm");
        }
    }
}
