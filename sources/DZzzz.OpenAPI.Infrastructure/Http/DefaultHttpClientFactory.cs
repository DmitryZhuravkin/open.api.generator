using System.Net;
using System.Net.Http;

namespace DZzzz.OpenAPI.Infrastructure.Http
{
    public class DefaultHttpClientFactory : SingleHttpClientFactory
    {
        public override HttpClient GetHttpClient()
        {
            HttpClientHandler handler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            //HttpLoggingHandler loggingHandler = new HttpLoggingHandler(handler, loggingService);
            HttpClient client = new HttpClient(handler);

            //client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            //client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en-US", 0.5));
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }
    }
}
