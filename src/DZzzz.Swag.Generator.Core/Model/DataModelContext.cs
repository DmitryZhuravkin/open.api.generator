using System.Collections.Generic;

namespace DZzzz.Swag.Generator.Core.Model
{
    public class DataModelContext
    {
        public string ID { get; set; }

        public Dictionary<string, Parameter> Propeties { get; }

        public List<string> SubTypes { get; }

        public DataModelContext()
        {
            Propeties = new Dictionary<string, Parameter>();
            SubTypes = new List<string>();
        }
    }
}