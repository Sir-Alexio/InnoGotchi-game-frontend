using InnoGotchi_backend.Models;
using InnoGotchi_frontend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace InnoGotchi_frontend.Controllers
{
    [Route("account")]
    public class AccountDetailController : Controller
    {
        private readonly HttpClient _httpClient;

        public AccountDetailController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("Client");
        }
        /// <summary>
        /// How get personal-info from here
        /// error here
        /// </summary>
        /// <returns></returns>
        [Route("personal-info")]
        public async Task<IActionResult> Index()
        {           
            HttpResponseMessage response = await _httpClient.GetAsync($"api/authorization/user/{Request.Cookies["token"]}");

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest("Error 404");
            }

            var jsonUser = response.Content.ReadAsStringAsync().Result;

            UserDto? user =  JsonSerializer.Deserialize<UserDto>(jsonUser);

            return View("Index",user);
        }
        [Route("update")]
        public IActionResult Update(RegistrationUser registrationUser)
        {
            UserDto dto = registrationUser.Dto;

            return View();
        }
    }
}
