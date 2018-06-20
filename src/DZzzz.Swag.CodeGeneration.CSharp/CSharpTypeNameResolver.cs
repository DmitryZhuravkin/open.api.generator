using DZzzz.Swag.CodeGeneration.CSharp.Common;
using DZzzz.Swag.Generator.Core.Model;

namespace DZzzz.Swag.CodeGeneration.CSharp
{
    public class CSharpTypeNameResolver
    {
        public string ResolveType(Parameter parameter)
        {
            if (parameter.IsCollectionParameter)
            {
                return $"List<{ResolveType(parameter.Type, parameter.Format)}>";
            }

            return ResolveType(parameter.Type, parameter.Format);
        }

        private string ResolveType(string type, string format)
        {
            switch (type)
            {
                case "integer":
                    return format.ToCamelCase();
                case "number":
                    return format.ToCamelCase();
                case "string":
                    if (format == "byte")
                    {
                        return "Byte";
                    }
                    else if (format == "date" || format == "date-time")
                    {
                        return "DateTime";
                    }

                    break;
            }

            return type.ToCamelCase();
        }
    }
}