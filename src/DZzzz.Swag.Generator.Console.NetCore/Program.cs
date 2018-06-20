using DZzzz.Swag.CodeGeneration.CSharp;
using DZzzz.Swag.CodeGeneration.CSharp.Configuration;
using DZzzz.Swag.Generator.Core.Model;
using DZzzz.Swag.Specification.Base;
using DZzzz.Swag.Specification.Version12;

namespace DZzzz.Swag.Generator.Console.NetCore
{
    class Program
    {
        static void Main(string[] args)
        {
            Version12SpecificationProvider provider = new Version12SpecificationProvider(new SwagSpecificationContext()
            {
                Url = "http://209.201.33.16:8080/docs/aq-api/apidocs/service.json",
                Format = SwagFormat.Json
            });

            GenerationContext context = provider.GetSpecificationAsync().Result;

            CSharpLanguageCodeGenerator codeGenerator = new CSharpLanguageCodeGenerator(new CSharpLanguageSettings
            {
                OutputFolder = @"E:\development.active\agilquest\sources\AgilQuest.Net",
                OutputProjectName = "AgilQuest.Net.Phoenix",
                FileNamePrefix = "Phoenix"
            });

            codeGenerator.Generate(context);
        }
    }
}
