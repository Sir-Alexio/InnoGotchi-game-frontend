using InnoGotchi_backend.Models.Entity;
using InnoGotchi_frontend.Services.Abstract;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
namespace InnoGotchi_frontend.Services
{
    public class TokenService : ITokenService
    {
        private readonly HttpClient _httpClient;
        public TokenService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("Client");
        }
        public void AddTokenToCookie(string token, HttpContext context,string tokenName,int exipierDays)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(exipierDays)
            };

            context.Response.Cookies.Append(tokenName, token, cookieOptions);
        }

        public void RemoveTokenFromCookie(HttpContext context)
        {
            var options = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(-1)
            };

            context.Response.Cookies.Delete("token", options);
        }

        public async Task<string> RefreshTokenAsync(HttpContext context)
        {

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", context.Request.Cookies["token"]);

            string refreshToken = context.Request.Cookies["refreshToken"];

            // Prepare the refresh token request
            HttpResponseMessage response = await _httpClient.PostAsync($"api/authorization/refresh-token?refreshToken={refreshToken}", null);

            if (!response.IsSuccessStatusCode)
            {
                // Extract the new access token from the response
                throw new CustomExeption("No new Acces token found");
            }

            string newAccessToken = await response.Content.ReadAsStringAsync();

            return newAccessToken;  
        }

        public bool IsTokenValid(HttpContext context)
        {
            string? token = context.Request.Cookies["token"];

            if (string.IsNullOrWhiteSpace(token)) { throw new CustomExeption("no token"); }

            var tokenHandler = new JwtSecurityTokenHandler();

            // Read the JWT token
            var jwtToken = tokenHandler.ReadJwtToken(token);

            // Get the token's expiration date/time
            DateTime expirationDateTime = jwtToken.ValidTo;
            expirationDateTime = expirationDateTime.AddDays(1);
            // Check if the token has expired
            return !(expirationDateTime < DateTime.UtcNow);
        }
    }
}
