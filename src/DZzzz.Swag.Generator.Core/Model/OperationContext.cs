using System.Collections.Generic;

namespace DZzzz.Swag.Generator.Core.Model
{
    public class OperationContext
    {
        public string Name { get; set; }

        public string RelativeUrl { get; set; }

        public string Method { get; set; }

        public string ReturnTypeName { get; set; }

        public Parameter BodyParameter { get; set; }

        public IList<Parameter> QueryParameters { get; }

        public IList<Parameter> PathParameters { get; }

        public OperationContext()
        {
            QueryParameters = new List<Parameter>();
            PathParameters = new List<Parameter>();
        }
    }
}