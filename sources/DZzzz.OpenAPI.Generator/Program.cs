using System;

using DZzzz.OpenAPI.Core.Model;
using DZzzz.OpenAPI.Infrastructure.Http;
using DZzzz.OpenAPI.Infrastructure.Serialization;

namespace DZzzz.OpenAPI.Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            DefaultHttpClientFactory clientFactory = new DefaultHttpClientFactory();
            NewtonJsonSerializer serializer = new NewtonJsonSerializer();

            HttpCommunicationService testService = new HttpCommunicationService(clientFactory, serializer);

            SwaggerModel model = testService.SendRequestAsync<SwaggerModel>("http://209.201.33.129:8080/docs/aq-api/apidocs/service.json").Result;



            Console.ReadKey(true);
        }
    }
}
