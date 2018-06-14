using System;

namespace DZzzz.OpenAPI.Core.Model
{
    public class OperationContext
    {
        public string RelativeUrl { get; set; }

        public Type InputType { get; set; }
    }
}