using InnoGotchi_backend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace InnoGotchi_frontend.Controllers
{
    [Route("account")]
    public class AccountDetailController : Controller
    {
        IHttpClientFactory _httpClientFactory;
        public AccountDetailController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IActionResult> Index()
        {
            HttpClient client = _httpClientFactory.CreateClient("Client");
            
            string? token = Request.Cookies["token"];

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("No token");
            }

            HttpResponseMessage response = await client.GetAsync($"api/authorization/user/{token}");

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest("Error 404");
            }

            var jsonUser = response.Content.ReadAsStringAsync().Result;

            UserDto? user1 =  JsonSerializer.Deserialize<UserDto>(jsonUser);

            UserDto newUserTest = new UserDto
            {
                UserName = "2132113",
                FirstName = "alexa",
                LastName = "mokarova",
                Avatar = "no Avatar",
                Password = "qweqwe",
                Email = "rtymail.com"
            };
            string jsonNewUser = JsonSerializer.Serialize(newUserTest);

            UserDto? user2 = JsonSerializer.Deserialize<UserDto>(jsonNewUser);

            return View("Index",user1);
        }
    }
}
