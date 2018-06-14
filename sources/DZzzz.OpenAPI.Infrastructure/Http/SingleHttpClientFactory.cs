using System.Net.Http;

namespace DZzzz.OpenAPI.Infrastructure.Http
{
    public abstract class SingleHttpClientFactory : IHttpClientFactory
    {
        private HttpClient httpClient;
        private readonly object lockObject = new object();

        public HttpClient CreateClient()
        {
            if (httpClient == null)
            {
                lock (lockObject)
                {
                    if (httpClient == null)
                    {
                        httpClient = GetHttpClient();
                    }
                }
            }

            return httpClient;
        }

        public void Dispose()
        {
            if (httpClient != null)
            {
                lock (lockObject)
                {
                    if (httpClient != null)
                    {
                        try
                        {
                            httpClient.Dispose();

                        }
                        finally
                        {
                            httpClient = null;
                        }
                    }
                }
            }
        }

        public abstract HttpClient GetHttpClient();
    }
}
