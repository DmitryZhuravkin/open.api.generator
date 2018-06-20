using System.Collections.Generic;

using Newtonsoft.Json;

namespace DZzzz.Swag.Specification.Version12.Model.Declaration
{
    public class DataTypeObject
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("$ref")]
        public string ReferenceTypeID { get; set; }

        [JsonProperty("format")]
        public string Format { get; set; }

        [JsonProperty("enum")]
        public List<string> PossibleValues { get; set; } = new List<string>();

        [JsonProperty("items")]
        public ItemsObject Items { get; set; }
    }
}