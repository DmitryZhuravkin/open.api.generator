using System.Collections.Generic;

namespace DZzzz.Swag.Generator.Core.Model
{
    public class GenerationContext
    {
        public IDictionary<string, DataModelContext> DataModels { get; }

        public IList<OperationGroupContext> Groups { get; }

        public GenerationContext()
        {
            Groups = new List<OperationGroupContext>();
            DataModels = new Dictionary<string, DataModelContext>();
        }
    }
}