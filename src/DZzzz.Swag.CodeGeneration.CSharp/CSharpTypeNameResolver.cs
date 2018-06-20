using System;
using System.Text.RegularExpressions;

using DZzzz.Swag.CodeGeneration.CSharp.Common;
using DZzzz.Swag.Generator.Core.Model;

namespace DZzzz.Swag.CodeGeneration.CSharp
{
    public class CSharpTypeNameResolver
    {
        public string ResolveType(TypeContext parameter)
        {
            if (parameter != null)
            {
                if (parameter.IsCollectionParameter)
                {
                    return $"List<{ResolveTypeInternal(parameter)}>";
                }

                return ResolveTypeInternal(parameter);
            }

            return string.Empty;
        }

        public string FixPossibleTypeNameIssues(string type)
        {
            string typeName = type.Trim().Replace(" ", "");

            return RemoveDashes(typeName).ToCamelCase();
        }

        private string ResolveTypeInternal(TypeContext parameter)
        {
            if (!String.IsNullOrEmpty(parameter.Type))
            {
                switch (parameter.Type)
                {
                    case "integer":
                        return parameter.Format.ToCamelCase() + "?";
                    case "number":
                        return parameter.Format.ToCamelCase() + "?";
                    case "string":
                        if (parameter.Format == "byte")
                        {
                            return "Byte?";
                        }
                        else if (parameter.Format == "date" || parameter.Format == "date-time")
                        {
                            return "DateTime?";
                        }

                        break;
                }

                return FixPossibleTypeNameIssues(parameter.Type);
            }

            return parameter.Type;
        }

        private string RemoveDashes(string value)
        {
            Regex regex = new Regex(@"-(?<middle>\w)");

            return regex.Replace(value, Evaluator);
        }

        private string Evaluator(Match match)
        {
            if (match.Success)
            {
                return match.Groups["middle"].Value.ToUpper();
            }

            return match.Groups["middle"].Value;
        }
    }
}