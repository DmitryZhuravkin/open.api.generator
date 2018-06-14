using Newtonsoft.Json;

namespace DZzzz.OpenAPI.Version12.Model
{
    public class Parameter
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("paramType")]
        public string ParamType { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("required")]
        public bool Required { get; set; }

        [JsonProperty("format")]
        public string Format { get; set; }
    }
}
