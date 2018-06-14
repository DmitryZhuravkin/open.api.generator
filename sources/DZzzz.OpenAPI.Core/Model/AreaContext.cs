using System.Collections.Generic;

namespace DZzzz.OpenAPI.Core.Model
{
    public class AreaContext
    {
        public string Name { get; set; }

        public List<OperationContext> Operations { get; set; }
    }
}