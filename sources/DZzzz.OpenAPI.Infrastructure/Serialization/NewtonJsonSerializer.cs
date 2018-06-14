using DZzzz.OpenAPI.Core.Interfaces;

using Newtonsoft.Json;

namespace DZzzz.OpenAPI.Infrastructure.Serialization
{
    public class NewtonJsonSerializer : ISerializer<string>
    {
        #region fields

        private readonly JsonSerializerSettings settings;

        #endregion
        #region methods

        #region ctor/dtor

        public NewtonJsonSerializer()
        {
            settings = new JsonSerializerSettings
            {
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                DateParseHandling = DateParseHandling.None,
                NullValueHandling = NullValueHandling.Ignore
            };
        }

        public NewtonJsonSerializer(JsonSerializerSettings settings)
        {
            this.settings = settings;
        }

        #endregion

        public string Serialize<TK>(TK @object)
        {
            return JsonConvert.SerializeObject(@object, settings);
        }

        public TK Deserialize<TK>(string value)
        {
            return JsonConvert.DeserializeObject<TK>(value, settings);
        }

        #endregion
    }
}
