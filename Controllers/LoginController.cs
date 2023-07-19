using Microsoft.AspNetCore.Mvc;
using InnoGotchi_backend.Models.Dto;
using InnoGotchi_frontend.Services.Abstract;
using InnoGotchi_frontend.Models.Validators;

namespace InnoGotchi_frontend.Controllers
{
    [Route("login")]
    public class LoginController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenService _tokenService;

        public LoginController(
            IHttpClientFactory httpClientFactory,
            ITokenService tokenManager)
        {
            _httpClient = httpClientFactory.CreateClient("Client");
            _tokenService = tokenManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LogIn(UserDto dto)
        {
            UserValidator validator = new UserValidator();

            if (!validator.Validate(dto).IsValid)
            {
                return View("Index", dto);
            }

            JsonContent content = JsonContent.Create(dto);

            HttpResponseMessage response = await _httpClient.PostAsync("api/authorization/login", content);

            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();
                
                ViewBag.Message = error;

                return View("Index", dto);
            }

            _tokenService.AddTokenToCookie(await response.Content.ReadAsStringAsync(),HttpContext,"token",1);

            return RedirectToAction("personal-info", "account");
        }
        
    }
}
