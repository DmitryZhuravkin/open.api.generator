using System.Collections.Generic;

using Newtonsoft.Json;

namespace DZzzz.OpenAPI.Version12.Model
{
    public class Operation
    {
        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("parameters")]
        public List<Parameter> Parameters { get; set; }

        [JsonProperty("consumes")]
        public List<string> Consumes { get; set; }

        [JsonProperty("produces")]
        public List<string> Produces { get; set; }
    }
}
