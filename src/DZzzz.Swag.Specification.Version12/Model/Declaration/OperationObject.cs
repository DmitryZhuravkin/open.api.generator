using System.Collections.Generic;

using Newtonsoft.Json;

namespace DZzzz.Swag.Specification.Version12.Model.Declaration
{
    public class OperationObject : DataTypeObject
    {
        [JsonProperty("method")]
        public Method Method { get; set; }

        [JsonProperty("nickname")]
        public string NickName { get; set; }

        [JsonProperty("parameters")]
        public List<ParameterObject> Parameters { get; set; } = new List<ParameterObject>();
    }
}