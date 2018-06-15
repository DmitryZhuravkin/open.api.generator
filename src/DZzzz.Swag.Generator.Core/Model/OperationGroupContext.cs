using System.Collections.Generic;

namespace DZzzz.Swag.Generator.Core.Model
{
    public class OperationGroupContext
    {
        public string Name { get; set; }

        public IList<OperationContext> Operations { get; }

        public OperationGroupContext()
        {
            Operations = new List<OperationContext>();
        }
    }
}