using InnoGotchi_backend.Models.Dto;
using InnoGotchi_backend.Models.DTOs;
using InnoGotchi_frontend.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace InnoGotchi_frontend.Controllers
{
    [Route("user")]
    public class UserController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenService _tokenService;
        public UserController(IHttpClientFactory factory, ITokenService tokenService)
        {
            _httpClient = factory.CreateClient("Client");
            _tokenService = tokenService;
        }

        [Route("all-users")]
        public async Task<IActionResult> GetAllUsers()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["token"]);

            HttpResponseMessage response = await _httpClient.GetAsync("api/user/users-with-no-invited");

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest();
            }

            List<UserDto>? users = JsonSerializer.Deserialize<List<UserDto>>(response.Content.ReadAsStringAsync().Result);

            return View("AllUsers", users);
        }

        [Route("invite-friend/{email}")]
        public async Task<IActionResult> InviteFriend(string email)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["token"]);

            JsonContent content = JsonContent.Create(new { Email = email });

            HttpResponseMessage response = await _httpClient.PatchAsync($"api/user/invite-user?inviteUserEmail={email}", null);

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest();
            }

            return RedirectToAction("all-users","user");
        }
    }
}
