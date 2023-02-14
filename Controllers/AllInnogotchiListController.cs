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

namespace InnoGotchi_frontend.Controllers
{
    [Route("innogotches")]
    public class AllInnogotchiListController : Controller
    {
        private readonly HttpClient _client;

        public AllInnogotchiListController(IHttpClientFactory clientFactory)
        {
            _client = clientFactory.CreateClient("Client");
        }

        public async Task<ActionResult> Index()
        {
            IRequestCookieCollection cookie = HttpContext.Request.Cookies;

            if (cookie.TryGetValue("token",out string? token))
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            HttpResponseMessage response = await _client.GetAsync("api/pet");
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest("UnAuthorized");
            }
            return Ok(response);
            
        }

        
    }
}
