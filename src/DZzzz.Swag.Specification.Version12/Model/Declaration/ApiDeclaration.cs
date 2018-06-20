using System.Collections.Generic;

using Newtonsoft.Json;

namespace DZzzz.Swag.Specification.Version12.Model.Declaration
{
    public class ApiDeclaration
    {
        [JsonProperty("swaggerVersion")]
        public string SwaggerVersion { get; set; }

        [JsonProperty("apiVersion")]
        public string ApiVersion { get; set; }

        [JsonProperty("basePath")]
        public string BasePath { get; set; }

        [JsonProperty("resourcePath")]
        public string ResourcePath { get; set; }

        [JsonProperty("apis")]
        public List<ApiObject> Apis { get; set; }

        [JsonProperty("models")]
        public Dictionary<string, ModelObject> Models { get; set; } = new Dictionary<string, ModelObject>();
    }
}