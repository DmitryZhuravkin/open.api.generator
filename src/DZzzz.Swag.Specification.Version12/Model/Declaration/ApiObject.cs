using System.Collections.Generic;
using Newtonsoft.Json;

namespace DZzzz.Swag.Specification.Version12.Model.Declaration
{
    public class ApiObject
    {
        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("operations")]
        public List<OperationObject> Operations { get; set; } = new List<OperationObject>();
    }
}