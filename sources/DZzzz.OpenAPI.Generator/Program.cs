using System;

using DZzzz.OpenAPI.Infrastructure.Http;

namespace DZzzz.OpenAPI.Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            DefaultHttpClientFactory clientFactory = new DefaultHttpClientFactory();
            HttpCommunicationService testService = new HttpCommunicationService(clientFactory);

            testService.SendRequestAsync<object>("http://209.201.33.129:8080/docs/aq-api/apidocs/service.json").Wait();

            Console.ReadKey(true);
        }
    }
}
