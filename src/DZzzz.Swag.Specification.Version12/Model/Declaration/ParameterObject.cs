using Newtonsoft.Json;

namespace DZzzz.Swag.Specification.Version12.Model.Declaration
{
    public class ParameterObject : DataTypeObject
    {
        [JsonProperty("paramType")]
        public ParamType ParamType { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("required")]
        public bool Required { get; set; }

        [JsonProperty("allowMultiple")]
        public bool AllowMultiple { get; set; }
    }
}