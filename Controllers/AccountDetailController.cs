using InnoGotchi_backend.Models.Dto;
using InnoGotchi_backend.Models.Entity;
using InnoGotchi_frontend.Models;
using InnoGotchi_frontend.Models.Validators;
using InnoGotchi_frontend.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Text.Json;


namespace InnoGotchi_frontend.Controllers
{
    [Route("account")]
    public class AccountDetailController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IWebHostEnvironment _environment;
        private readonly ITokenService _tokenService;

        private static UserDto _user;

        public AccountDetailController(IHttpClientFactory httpClientFactory,
            IWebHostEnvironment environment,
            ITokenService tokenService)
        {
            _httpClient = httpClientFactory.CreateClient("Client");
            _environment = environment;
            _tokenService = tokenService;
        }

        [Route("personal-info")]
        public async Task<IActionResult> Index()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["token"]);
            
            HttpResponseMessage response = await _httpClient.GetAsync($"api/authorization/user");

            if (!response.IsSuccessStatusCode)
            {
                CustomExeption? errorMessage = JsonSerializer.Deserialize<CustomExeption>(response.Content.ReadAsStringAsync().Result);

                ViewBag.Message = errorMessage.Message;

                return View("personal-info", _user);
            }

            string jsonUser = response.Content.ReadAsStringAsync().Result;

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
            //костыль
            registrationUser.Dto.Password = "hiden";

            _user = registrationUser.Dto;

            UserValidator validator = new UserValidator();

            if (!validator.Validate(_user).IsValid)
            {
                return View("Update", registrationUser);
            }

            _user.Avatar = UpdateUploadedImage(registrationUser).Result;

            JsonContent content = JsonContent.Create(_user);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["token"]);

            HttpResponseMessage response = await _httpClient.PatchAsync("api/account/modify-user", content);

            if (!response.IsSuccessStatusCode)
            {
                CustomExeption? errorMessage = JsonSerializer.Deserialize<CustomExeption>(response.Content.ReadAsStringAsync().Result);

                ViewBag.Message = errorMessage.Message;

                return View("Update", registrationUser);
            }
            return View("personal-info", _user);
        }

        [Route("change-password")]
        public async Task<ActionResult> ChangePassword(ChangePasswordModel model, RegistrationUser registrationUser)
        {
            ChangePasswordValidator validator = new ChangePasswordValidator();

            if (!validator.Validate(model).IsValid)
            {
                return View("ChangePassword", model);
            }

            //refresh token
            //if (!_tokenService.IsTokenValid(context: HttpContext)) { _tokenService.AddTokenToCookie(await _tokenService.RefreshTokenAsync(HttpContext), HttpContext, "token", 1); }

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
