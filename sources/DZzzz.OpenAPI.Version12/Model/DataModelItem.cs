using System.Collections.Generic;

using Newtonsoft.Json;

namespace DZzzz.OpenAPI.Version12.Model
{
    public class DataModelItem
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("properties")]
        public Dictionary<string, DataModelPropertyItem> Properties { get; set; }
    }
}