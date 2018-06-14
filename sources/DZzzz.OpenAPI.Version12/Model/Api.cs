using Newtonsoft.Json;

using System.Collections.Generic;

namespace DZzzz.OpenAPI.Version12.Model
{
    public class Api
    {
        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("operations")]
        public List<Operation> Operations { get; set; }
    }
}
