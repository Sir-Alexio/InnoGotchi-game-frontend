using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using InnoGotchi_backend.Models.DTOs;

namespace InnoGotchi_frontend.Controllers
{
    [Route("farm-detail")]
    public class FarmDetailController : Controller
    {
        private readonly HttpClient _httpClient;
        public FarmDetailController(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("Client");
        }

        [Route("main-page")]
        public IActionResult GetFarmDetailPage()
        {
            return View("FarmDetailPage");
        }
        [Route("statistics")]
        public async Task<IActionResult> GetFarmStatisticsPage()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["token"]);

            HttpResponseMessage response = await _httpClient.GetAsync($"api/farm/statistic");

            StatisticDto? statistic = JsonSerializer.Deserialize<StatisticDto>(await response.Content.ReadAsStringAsync());

            if (statistic == null)
            {
                return RedirectToAction("mail-page", "farm - detail");
            }

            return View("Statistic", statistic);
        }
    }
}
