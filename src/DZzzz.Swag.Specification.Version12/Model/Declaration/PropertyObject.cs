using Newtonsoft.Json;

namespace DZzzz.Swag.Specification.Version12.Model.Declaration
{
    public class PropertyObject : DataTypeObject
    {
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}