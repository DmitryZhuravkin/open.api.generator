using Newtonsoft.Json;

namespace DZzzz.Swag.Specification.Version12.Model.Declaration
{
    public class ItemsObject
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("$ref")]
        public string ReferenceTypeID { get; set; }

        [JsonProperty("format")]
        public string Format { get; set; }
    }
}