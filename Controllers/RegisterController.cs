using FluentValidation;
using InnoGotchi_backend.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using FluentValidation.Results;
using FluentValidation.AspNetCore;
using InnoGotchi_frontend.Services;
using InnoGotchi_frontend.Models;

namespace InnoGotchi_frontend.Controllers
{
    public class RegisterController : Controller,IValidationController
    {
        private readonly HttpClient _httpClient;
        private readonly IValidator<UserDto> _validator;

        public RegisterController(IHttpClientFactory httpClientFactory, IValidator<UserDto> validator)
        {
            _httpClient = httpClientFactory.CreateClient("Client");
            _validator = validator;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> OnPost(UserDto userDto)
        {
            ValidationResult validationResult = await _validator.ValidateAsync(userDto);

            if (!Validation(userDto).Result){
                return View("Index", userDto);
            }
            
            JsonContent content = JsonContent.Create(userDto);

            using HttpResponseMessage response = await _httpClient.PostAsync("api/registration", content);

            if (!response.IsSuccessStatusCode)
            {
                this.ModelState.AddModelError(string.Empty, "This Email is already exist");

                validationResult.AddToModelState(this.ModelState);

                return View("Index", userDto);   
            }

            return RedirectToAction("Index", "Login");

        }

        public async Task<bool> Validation(UserDto userDto)
        {
            ValidationResult validationResult = await _validator.ValidateAsync(userDto);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(this.ModelState);
                
                return false;
            }

            return true;
        }
    }
}
