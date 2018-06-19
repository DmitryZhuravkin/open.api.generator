using System;

using NJsonSchema;
using NJsonSchema.CodeGeneration.CSharp;

namespace DZzzz.JsonSchemaToCSharp.Tool
{
    class Program
    {
        public static void Main(string[] args)
        {
            JsonSchema4 schema = JsonSchema4.FromFileAsync("apiDeclaration.json").Result;

            CSharpGeneratorSettings settings = new CSharpGeneratorSettings
            {
                Namespace = "DZzzz.Swag.Specification.Version12",
                GenerateDataAnnotations = false,
                GenerateDefaultValues = false,
                GenerateJsonMethods = false,
                ArrayBaseType = "System.Collections.Generic.List",
                ArrayType = "System.Collections.Generic.List",
                ClassStyle = CSharpClassStyle.Poco,
                RequiredPropertiesMustBeDefined = false
            };

            CSharpGenerator generator = new CSharpGenerator(schema, settings);

            var file = generator.GenerateFile();

            Console.ReadKey(true);
        }
    }
}
