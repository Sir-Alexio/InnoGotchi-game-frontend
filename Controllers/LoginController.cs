using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System;
using FluentValidation.Results;
using FluentValidation.AspNetCore;
using InnoGotchi_frontend.Services;
using NuGet.Common;
using InnoGotchi_backend.Models.Dto;

namespace InnoGotchi_frontend.Controllers
{
    public class LoginController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenManager _tokenManager;
        private readonly IValidationService _validation;

        public LoginController(
            IHttpClientFactory httpClientFactory,
            ITokenManager tokenManager,
            IValidationService validation)
        {
            _httpClient = httpClientFactory.CreateClient("Client");
            _tokenManager = tokenManager;
            _validation = validation;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LogIn(UserDto dto)
        {

            if (!_validation.Validation(dto,this.ModelState).Result)
            {
                return View("Index",dto);
            }

            JsonContent content = JsonContent.Create(dto);

            HttpResponseMessage response = await _httpClient.PostAsync("api/authorization", content);

            if (!response.IsSuccessStatusCode)
            {
                await _validation.AddError(dto,"Wrong email or password",this.ModelState);
                return View("Index", dto);
            }

            _tokenManager.AddTokenToCookie(response.Content.ReadAsStringAsync().Result,HttpContext);
            
            return RedirectToAction("personal-info", "account");
        }
        
    }
}
