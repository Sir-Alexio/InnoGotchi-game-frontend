using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System;
using FluentValidation.Results;
using FluentValidation.AspNetCore;
using InnoGotchi_frontend.Services;
using InnoGotchi_frontend.Models;
using Microsoft.Extensions.Hosting;
using InnoGotchi_backend.Models.Dto;

namespace InnoGotchi_frontend.Controllers
{
    public class RegisterController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IValidationService _validationService;
        private readonly IWebHostEnvironment _environment;

        public RegisterController(IHttpClientFactory httpClientFactory, IValidationService validation, IWebHostEnvironment environment)
        {
            _httpClient = httpClientFactory.CreateClient("Client");
            _validationService = validation;
            _environment = environment;
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

            if (!_validationService.Validation(dto, this.ModelState).Result)
            {
                return View("Index", registrationUser);
            }
            
            JsonContent content = JsonContent.Create(dto);

            using HttpResponseMessage response = await _httpClient.PostAsync("api/account/registration", content);

            if (!response.IsSuccessStatusCode)
            {
                await _validationService.AddError(dto,"This Email is already exist", this.ModelState);

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
