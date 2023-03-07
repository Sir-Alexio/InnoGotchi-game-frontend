using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System;
using FluentValidation.Results;
using FluentValidation.AspNetCore;
using NuGet.Common;
using InnoGotchi_backend.Models.Dto;
using InnoGotchi_frontend.Services.Abstract;
using InnoGotchi_frontend.Models;
using InnoGotchi_frontend.Models.Validators;
using InnoGotchi_backend.Models;
using System.Text.Json;

namespace InnoGotchi_frontend.Controllers
{
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

            HttpResponseMessage response = await _httpClient.PostAsync("api/authorization", content);

            if (!response.IsSuccessStatusCode)
            {
                CustomExeption? errorMessage = JsonSerializer.Deserialize<CustomExeption>(response.Content.ReadAsStringAsync().Result);

                ViewBag.Message = errorMessage.Message;

                return View("Index", dto);
            }

            _tokenService.AddTokenToCookie(response.Content.ReadAsStringAsync().Result,HttpContext);
            
            return RedirectToAction("personal-info", "account");
        }
        
    }
}
