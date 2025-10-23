using System.Net.Http.Headers;

namespace WebApp.Services.Base
{
    public abstract class AbstractService
    {
        private const string ApiBaseUri = "https://localhost:7063/";
        private readonly IHttpClientFactory factory;
        private readonly IHttpContextAccessor httpContextAccessor;

        protected AbstractService(IHttpClientFactory factory, IHttpContextAccessor httpContextAccessor)
        {
            this.factory = factory;
            this.httpContextAccessor = httpContextAccessor;
        }

        protected HttpClient CreateClient()
        {
            var client = this.factory.CreateClient();
            client.BaseAddress = new Uri(ApiBaseUri);

            var jwtToken = this.httpContextAccessor.HttpContext?.Request.Cookies["jwt"];
            if (!string.IsNullOrEmpty(jwtToken))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", jwtToken);
            }

            return client;
        }
    }
}
