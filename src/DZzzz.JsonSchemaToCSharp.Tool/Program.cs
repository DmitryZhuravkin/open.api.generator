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

            CSharpGenerator generator = new CSharpGenerator(schema, new CSharpGeneratorSettings());
            var file = generator.GenerateFile();

            Console.ReadKey(true);
        }
    }
}
