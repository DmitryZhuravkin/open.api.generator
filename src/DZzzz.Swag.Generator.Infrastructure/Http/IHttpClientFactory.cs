using System.Net.Http;

namespace DZzzz.Swag.Generator.Infrastructure.Http
{
    public interface IHttpClientFactory
    {
        HttpClient CreateClient();
    }
}
