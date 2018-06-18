using System;
using System.Net.Http;
using System.Threading.Tasks;

using DZzzz.Swag.Generator.Core.Interfaces;

namespace DZzzz.Swag.Generator.Infrastructure.Http
{
    public class HttpCommunicationService : ICommunicationService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ISerializer<string> serializer;

        public HttpCommunicationService(IHttpClientFactory httpClientFactory, ISerializer<string> serializer)
        {
            this.httpClientFactory = httpClientFactory;
            this.serializer = serializer;
        }

        public async Task<T> SendRequestAsync<T>(string url)
        {
            HttpClient client = httpClientFactory.CreateClient();

            Uri relativeUri = new Uri(url);

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, relativeUri))
            {
                using (HttpResponseMessage message = await client.SendAsync(request).ConfigureAwait(false))
                {
                    message.EnsureSuccessStatusCode();

                    string stringContent = await message.Content.ReadAsStringAsync().ConfigureAwait(false);

                    if (!String.IsNullOrEmpty(stringContent))
                    {
                        return serializer.Deserialize<T>(stringContent);
                    }

                    return default(T);
                }
            }
        }
    }
}
