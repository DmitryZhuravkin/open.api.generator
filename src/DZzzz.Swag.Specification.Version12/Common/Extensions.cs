using System.Net.Http;

namespace DZzzz.Swag.Specification.Version12.Common
{
    public static class Extensions
    {
        public static HttpMethod ToHttpMethod(this OperationObjectMethod operationObject)
        {
            string stringHttp = operationObject.ToString();

            return new HttpMethod(stringHttp);
        }
    }
}
