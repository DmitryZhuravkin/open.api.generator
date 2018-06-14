using DZzzz.OpenAPI.Version12.Model;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace DZzzz.OpenAPI.Core.Model
{
    public class SwaggerModel
    {
        [JsonProperty("apiVersion")]
        public string ApiVersion { get; set; }

        [JsonProperty("swaggerVersion")]
        public string SwaggerVersion { get; set; }

        [JsonProperty("basePath")]
        public string BasePath { get; set; }

        [JsonProperty("resourcePath")]
        public string ResourcePath { get; set; }

        [JsonProperty("apis")]
        public List<Api> Apis { get; set; }

        //[JsonProperty("models")]
        //public Models Models { get; set; }
    }
}
