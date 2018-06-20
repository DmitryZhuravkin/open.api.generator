using System.Collections.Generic;

using Newtonsoft.Json;

namespace DZzzz.Swag.Specification.Version12.Model.Listing
{
    public class ResourceListing
    {
        [JsonProperty("swaggerVersion", Required = Required.Always)]
        public string SwaggerVersion { get; set; }

        [JsonProperty("apiVersion")]
        public string ApiVersion { get; set; }

        [JsonProperty("apis", Required = Required.Always)]
        public List<Resource> Apis { get; set; } = new List<Resource>();
    }
}