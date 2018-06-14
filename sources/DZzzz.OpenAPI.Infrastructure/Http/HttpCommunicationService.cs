using System;
using System.Net.Http;
using System.Threading.Tasks;

using DZzzz.OpenAPI.Core.Interfaces;

namespace DZzzz.OpenAPI.Infrastructure.Http
{
    public class HttpCommunicationService : ICommunicationService
    {
        private readonly IHttpClientFactory httpClientFactory;

        public HttpCommunicationService(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<T> SendRequestAsync<T>(string url)
        {
            HttpClient client = httpClientFactory.CreateClient();

            Uri relativeUri = new Uri(url);

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, relativeUri))
            {
                using (HttpResponseMessage message = await client.SendAsync(request).ConfigureAwait(false))
                {
                    string stringContent = await message.Content.ReadAsStringAsync().ConfigureAwait(false);

                    //return serializer.Deserialize<TK>(stringContent);

                    return default(T);
                }
            }
        }
    }
}
