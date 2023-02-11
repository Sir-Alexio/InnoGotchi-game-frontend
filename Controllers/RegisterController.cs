using InnoGotchi_backend.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace InnoGotchi_frontend.Controllers
{
    public class RegisterController : Controller
    {
        private readonly HttpClient _httpClient;

        public RegisterController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("Client");
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> OnPost(UserDto userDto)
        {
            JsonContent content = JsonContent.Create(userDto);

            using HttpResponseMessage response = await _httpClient.PostAsync("api/Reg/registration", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Login");
            }

            return BadRequest("Problem with registration user");
     
        }
    }
}
