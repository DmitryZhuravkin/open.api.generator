using System.Collections.Generic;

namespace DZzzz.Swag.Generator.Core.Model
{
    public class DataModelContext
    {
        public string ID { get; set; }

        public IDictionary<string, Parameter> Propeties { get; }

        public DataModelContext()
        {
            Propeties = new Dictionary<string, Parameter>();
        }
    }
}