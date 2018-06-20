using Newtonsoft.Json;

namespace DZzzz.Swag.Specification.Version12.Model.Listing
{
    public class Resource
    {
        [JsonProperty("path", Required = Required.Always)]
        public string Path { get; set; }
    }
}