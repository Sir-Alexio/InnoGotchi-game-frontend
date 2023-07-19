using InnoGotchi_backend.Models.Dto;
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

            List<UserDto>? users = JsonSerializer.Deserialize<List<UserDto>>(await response.Content.ReadAsStringAsync());

            return View("AllUsers", users);
        }

        [Route("invite-friend/{email}")]
        public async Task<IActionResult> InviteFriend(string email)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["token"]);

            HttpResponseMessage response = await _httpClient.PatchAsync($"api/user/invite-user?inviteUserEmail={email}", null);

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest();
            }

            return RedirectToAction("all-users","user");
        }

        [Route("find-user")]
        public async Task<IActionResult> FindUserByEmail(string email)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["token"]);

            HttpResponseMessage response = await _httpClient.GetAsync($"api/user/find-user/{email}");

            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();

                ViewBag.Message = error;

                return View("AllUsers",new List<UserDto>());
            }

            List<UserDto>? users = JsonSerializer.Deserialize<List<UserDto>>(await response.Content.ReadAsStringAsync());

            return View("AllUsers", users);
        }

        [Route("collaborators")]
        public async Task<IActionResult> GetMyColaborators()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["token"]);

            HttpResponseMessage response = await _httpClient.GetAsync($"api/user/collaborators");

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest();
            }

            List<UserDto>? collaborators = JsonSerializer.Deserialize<List<UserDto>>(await response.Content.ReadAsStringAsync());

            return View("Collaborators", collaborators);
        }

        [Route("i-am-collaborator")]
        public async Task<IActionResult> GetUserIAmCollaborator()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["token"]);

            HttpResponseMessage response = await _httpClient.GetAsync($"api/user/i-am-collaborator");

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest();
            }

            List<UserDto>? iAmCollab = JsonSerializer.Deserialize<List<UserDto>>(await response.Content.ReadAsStringAsync());

            return View("IAmCollaborator", iAmCollab);
        }

        [Route("delete-collaborator/{email}")]
        public async Task<IActionResult> GetUserIAmCollaborator(string email)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["token"]);

            HttpResponseMessage response = await _httpClient.DeleteAsync($"api/user/collaborators/{email}");

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest();
            }

            return RedirectToAction("collaborators", "user");
        }

    }
}
