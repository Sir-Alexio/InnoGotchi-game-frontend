
using System.IdentityModel.Tokens.Jwt;

namespace InnoGotchi_frontend.Middleware
{
    public class TokenExpirationMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            // Get the access token from the request
            string accessToken = context.Request.Headers["Authorization"];

            // Perform the token expiration check
            bool isExpired = IsAccessTokenExpired(accessToken);

            if (isExpired)
            {
                // Token has expired
                // Perform the necessary action (e.g., redirect to login page)
                context.Response.Redirect("/login");
                return;
            }

            // Token is still valid
            // Continue processing the request
            await next(context);
        }

        private bool IsAccessTokenExpired(string accessToken)
        {
            // Your token validation logic goes here
            // Parse the token, check the expiration claim, and compare it with the current time
            
            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.ReadJwtToken(accessToken);

            DateTime expirationDateTime = token.ValidTo;

            return expirationDateTime < DateTime.UtcNow;

        }
    }
}
