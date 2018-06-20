using System.Collections.Generic;

using Newtonsoft.Json;

namespace DZzzz.Swag.Specification.Version12.Model.Declaration
{
    public class ModelObject
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("properties")]
        public Dictionary<string, PropertyObject> Properties { get; set; } = new Dictionary<string, PropertyObject>();

        [JsonProperty("subTypes")]
        public IList<string> SubTypes { get; set; } = new List<string>();

        [JsonProperty("discriminator")]
        public string Discriminator { get; set; }
    }
}