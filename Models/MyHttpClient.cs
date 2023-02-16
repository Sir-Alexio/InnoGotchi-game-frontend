namespace InnoGotchi_frontend.Models
{
    public class MyHttpClient
    {
        private static HttpClient _httpClient;

        private MyHttpClient()
        {

        }

        public static HttpClient GetInstance()
        {
            if (_httpClient == null)
            {
                SocketsHttpHandler socketsHandler = new SocketsHttpHandler
                {
                    PooledConnectionLifetime = TimeSpan.FromMinutes(2)
                };

                _httpClient = new HttpClient(socketsHandler);
                _httpClient.BaseAddress = new System.Uri("https://localhost:7198/");
            }

            return _httpClient;
        }
     
    }
}
