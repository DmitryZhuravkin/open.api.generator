using Newtonsoft.Json;

namespace DZzzz.OpenAPI.Version12.Model
{
    public class DataModelPropertyItem
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("format")]
        public string Format { get; set; }
    }
}