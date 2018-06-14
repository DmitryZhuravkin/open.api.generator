using System.Net.Http;

namespace DZzzz.OpenAPI.Infrastructure.Http
{
    public interface IHttpClientFactory
    {
        HttpClient CreateClient();
    }
}
