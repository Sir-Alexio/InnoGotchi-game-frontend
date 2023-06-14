using Microsoft.AspNetCore.Mvc;
using InnoGotchi_frontend.Models;
using InnoGotchi_backend.Models.Dto;
using InnoGotchi_frontend.Models.Validators;
using System.Text.Json;
using InnoGotchi_backend.Models.Entity;
using InnoGotchi_frontend.Services.Abstract;

namespace InnoGotchi_frontend.Controllers
{
    public class RegisterController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenService _tokenService;
        private readonly IWebHostEnvironment _environment;

        public RegisterController(IHttpClientFactory httpClientFactory, IWebHostEnvironment environment, ITokenService tokenSevice)
        {
            _httpClient = httpClientFactory.CreateClient("Client");
            _environment = environment;
            _tokenService = tokenSevice;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> OnPost(RegistrationUser registrationUser)
        {
            UserDto dto = registrationUser.Dto;

            dto.Avatar = UploadImage(registrationUser.Image).Result;

            UserValidator validator = new UserValidator();

            if (!validator.Validate(dto).IsValid)
            {
                return View("Index", registrationUser);
            }

            JsonContent content = JsonContent.Create(dto);

            using HttpResponseMessage response = await _httpClient.PostAsync("api/account/registration", content);

            if (!response.IsSuccessStatusCode)
            {
                CustomExeption? errorMessage = JsonSerializer.Deserialize<CustomExeption>(response.Content.ReadAsStringAsync().Result);

                ViewBag.Message = errorMessage.Message;

                return View("Index", registrationUser);
            }

            return RedirectToAction("Index", "Login");
        }

        private async Task<string> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return "No file selected.";

            // create a unique filename
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            // set the path to the file system
            var filePath = Path.Combine(_environment.WebRootPath, "Images/avatars", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileName;
        }

    }
}
