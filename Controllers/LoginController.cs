using FluentValidation;
using InnoGotchi_backend.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using FluentValidation.Results;
using FluentValidation.AspNetCore;
using InnoGotchi_frontend.Services;

namespace InnoGotchi_frontend.Controllers
{
    public class LoginController : Controller, IValidationController
    {
        private readonly HttpClient _httpClient;
        private readonly IValidator<UserDto> _validator;

        public LoginController(IHttpClientFactory httpClientFactory, IValidator<UserDto> validator)
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
        public async Task<ActionResult> LogIn(UserDto dto)
        {
            dto.FirstName = "Alexey";
            dto.LastName = "Mokharev";
            dto.UserName = "mamka28";
            if (!Validation(dto).Result)
            {
                return View("Index",dto);
            }
            JsonContent content = JsonContent.Create(dto);

            using HttpResponseMessage response = await _httpClient.PostAsync("api/Auth/login", content);

            return Ok(response);
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
