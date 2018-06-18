using System.Collections.Generic;
using System.Net.Http;

namespace DZzzz.Swag.Generator.Core.Model
{
    public class OperationContext
    {
        public string Name { get; set; }

        public string RelativeUrl { get; set; }

        public HttpMethod Method { get; set; }

        public string ReturnTypeName { get; set; }

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