using System.Collections.Generic;

namespace DZzzz.Swag.Generator.Core.Model
{
    public class OperationContext : TypeContext
    {
        public string Name { get; set; }

        public string RelativeUrl { get; set; }

        public string Method { get; set; }

        public ParameterContext BodyParameter { get; set; }

        public IList<ParameterContext> QueryParameters { get; }

        public IList<ParameterContext> PathParameters { get; }

        public OperationContext()
        {
            QueryParameters = new List<ParameterContext>();
            PathParameters = new List<ParameterContext>();
        }
    }
}