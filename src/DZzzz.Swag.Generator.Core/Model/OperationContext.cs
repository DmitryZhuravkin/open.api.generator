using System.Collections.Generic;

namespace DZzzz.Swag.Generator.Core.Model
{
    public class OperationContext
    {
        public string RelativeUrl { get; set; }

        public string Method { get; set; }

        public BodyParameter BodyParameter { get; set; }

        public IList<QueryParameter> QueryParameters { get; }

        public IList<PathParameter> PathParameters { get; }

        public OperationContext()
        {
            QueryParameters = new List<QueryParameter>();
            PathParameters = new List<PathParameter>();
        }
    }
}