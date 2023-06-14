using Microsoft.AspNetCore.Mvc;
using InnoGotchi_backend.Controllers;
using InnoGotchi_backend.DataContext;
using InnoGotchi_backend.Models;
using Microsoft.AspNetCore.Http;
using NuGet.Protocol;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System.Text.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using InnoGotchi_frontend.Services;
using InnoGotchi_frontend.Services.Abstract;

namespace InnoGotchi_frontend.Controllers
{
    [Route("innogotches")]
    public class AllInnogotchiListController : Controller
    {
        private readonly HttpClient _client;
        private readonly ITokenService _tokenService;

        public AllInnogotchiListController(IHttpClientFactory clientFactory, ITokenService tokenService)
        {
            _client = clientFactory.CreateClient("Client");
            _tokenService = tokenService;
        }

        public async Task<ActionResult> Index()
        {
            //refresh token
            //if (!_tokenService.IsTokenValid(context: HttpContext)) { _tokenService.AddTokenToCookie(await _tokenService.RefreshTokenAsync(HttpContext), HttpContext, "token", 1); }

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["token"]);

            HttpResponseMessage response = await _client.GetAsync("api/pet");

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest("UnAuthorized");
            }
            return RedirectToAction("Index", "account");
            
        }

        
    }
}
