using InnoGotchi_backend.Models;
using InnoGotchi_backend.Models.Dto;
using InnoGotchi_frontend.Models;
using InnoGotchi_frontend.Services;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using System.Net.Http.Headers;
using System.Text.Json;

namespace InnoGotchi_frontend.Controllers
{
    [Route("account")]
    public class AccountDetailController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IWebHostEnvironment _environment;
        private readonly IValidationService _validationService;
        private readonly IPasswordValidationService _PasswordService;

        private static UserDto _user;

        public AccountDetailController(IHttpClientFactory httpClientFactory,
            IWebHostEnvironment environment,
            IValidationService validation,
            IPasswordValidationService passwordService)
        {
            _httpClient = httpClientFactory.CreateClient("Client");
            _environment = environment;
            _validationService = validation;
            _PasswordService = passwordService;
        }

        [Route("personal-info")]
        public async Task<IActionResult> Index()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["token"]);

            HttpResponseMessage response = await _httpClient.GetAsync($"api/authorization/user");

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest("Error 404");
            }

            var jsonUser = response.Content.ReadAsStringAsync().Result;

            _user =  JsonSerializer.Deserialize<UserDto>(jsonUser);

            return View("personal-info",_user);
        }

        [Route("edit-view")]
        public IActionResult EditView(RegistrationUser registrationUser)
        {
            registrationUser.Dto = _user;

            return View("Update", registrationUser);
        }

        [Route("update")]
        public async Task<IActionResult> Update(RegistrationUser registrationUser)
        {
            registrationUser.Dto.Password = "hiden";

            _user = registrationUser.Dto;

            if (!_validationService.Validation(_user, this.ModelState).Result)
            {
                return View("Update", registrationUser);
            }

            _user.Avatar = UpdateUploadedImage(registrationUser).Result;

            JsonContent content = JsonContent.Create(_user);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["token"]);

            HttpResponseMessage response = await _httpClient.PatchAsync("api/account/modify-user", content);

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest("Error 404");
            }
            return View("personal-info", _user);
        }

        [Route("change-password")]
        public async Task<ActionResult> ChangePassword(ChangePasswordModel model, RegistrationUser registrationUser)
        {
            if (!_PasswordService.Validation(model,this.ModelState).Result)
            {
                return View("ChangePassword", model);
            }

            JsonContent content = JsonContent.Create(model);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["token"]);

            HttpResponseMessage response = await _httpClient.PatchAsync("api/account/change-password", content);
            
            if (!response.IsSuccessStatusCode)
            {
                CustomExeption? error = JsonSerializer.Deserialize<CustomExeption>(response.Content.ReadAsStringAsync().Result);

                return BadRequest(error.Message);
            }

            registrationUser.Dto = _user;

            return View("Update", registrationUser);
        }

        [Route("change-password-view")]
        public IActionResult ChangePasswordView()
        {
            return View("ChangePassword");
        }
        private async Task<string> UpdateUploadedImage(RegistrationUser registrationUser)
        {
            IFormFile file = registrationUser.Image;

            if (file == null || file.Length == 0)
                return registrationUser.Dto.Avatar;

            DeteleExistingFile(registrationUser.Dto.Avatar);

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

        private void DeteleExistingFile(string path)
        {
            var filePath = Path.Combine(_environment.WebRootPath, "Images/avatars", path);

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }
    }
}
