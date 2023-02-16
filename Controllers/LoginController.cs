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
        private readonly ILogger<LoginController> _logger;

        public LoginController(IHttpClientFactory httpClientFactory, IValidator<UserDto> validator, ILogger<LoginController> logger)
        {
            _httpClient = httpClientFactory.CreateClient("Client");
            _validator = validator;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LogIn(UserDto dto)
        {

            if (!Validation(dto).Result)
            {
                return View("Index",dto);
            }

            JsonContent content = JsonContent.Create(dto);

            using HttpResponseMessage response = await _httpClient.PostAsync("api/authorization", content);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Password is incorect");
                return View("Index", dto);
            }

            AddTokenToCookie(response.Content.ReadAsStringAsync().Result);
            RemoveCookie("token");
            return RedirectToAction("Index", "account");
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
        private void AddTokenToCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(1)
            };
            Response.Cookies.Append("token", token, cookieOptions);
        }
        private void RemoveCookie(string cookieName)
        {
            var options = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(-1)
            };
            Response.Cookies.Delete($"{cookieName}", options);
        }
    }
}
