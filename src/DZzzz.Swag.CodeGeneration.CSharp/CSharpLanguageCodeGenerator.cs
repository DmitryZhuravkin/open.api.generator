using System;
using System.IO;
using System.Linq;

using DZzzz.Swag.CodeGeneration.CSharp.Common;
using DZzzz.Swag.Generator.Core.Interfaces;
using DZzzz.Swag.Generator.Core.Model;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DZzzz.Swag.CodeGeneration.CSharp
{
    public class CSharpLanguageCodeGenerator : ILanguageCodeGenerator
    {
        private readonly CSharpLanguageSettings configuration;
        private readonly CSharpLanguageModelGenerator modelGenerator;

        public CSharpLanguageCodeGenerator(CSharpLanguageSettings configuration)
        {
            this.configuration = configuration;

            modelGenerator = new CSharpLanguageModelGenerator(configuration);
        }

        public void Generate(GenerationContext context)
        {
            modelGenerator.GenerateModelClasses(context);
            GenerateRepositoryBaseClass();

            foreach (OperationGroupContext operationGroupContext in context.Groups)
            {
                GenerateRepositoryClass(operationGroupContext);
            }
        }

        private void GenerateRepositoryBaseClass()
        {
        }

        private void GenerateRepositoryClass(OperationGroupContext operationGroupContext)
        {
            try
            {
                string className = $"{configuration.FileNamePrefix}{operationGroupContext.Name.NormalizeName()}Repository";
                string fileName = $"{className}.generated.cs";
                string fileLocation = Path.Combine(configuration.OutputFolder, $"{configuration.OutputProjectName}\\Repositories\\Generated\\{fileName}");

                CompilationUnitSyntax compilationUnit = SyntaxFactory.CompilationUnit();
                compilationUnit = compilationUnit
                    .AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System")))
                    .AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System.Net.Http")))
                    .AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System.Threading.Tasks")))
                    .AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName($"{configuration.OutputProjectName}.Model")))
                    .AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName($"{configuration.OutputProjectName}.Repositories.Base")));

                NamespaceDeclarationSyntax @namespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName($"{configuration.OutputProjectName}.Repositories")).NormalizeWhitespace();

                ConstructorDeclarationSyntax constructor = GenerateConstructor(className);

                ClassDeclarationSyntax classDeclaration = SyntaxFactory.ClassDeclaration(className)
                    .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                    .AddBaseListTypes(SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName($"{configuration.FileNamePrefix}RepositoryBase")))
                    .AddMembers(constructor);

                // TODO: generate constructor

                foreach (OperationContext operationContext in operationGroupContext.Operations)
                {
                    MethodDeclarationSyntax methodDeclaration = GenerateMethod(operationContext);

                    classDeclaration = classDeclaration.AddMembers(methodDeclaration);
                }

                @namespace = @namespace.AddMembers(classDeclaration);
                compilationUnit = compilationUnit.AddMembers(@namespace);

                string fileContent = compilationUnit.NormalizeWhitespace().ToFullString();

                File.WriteAllText(fileLocation, fileContent);
            }
            catch
            {
                // DO NOTHING
            }
        }

        private ConstructorDeclarationSyntax GenerateConstructor(string className)
        {
            ConstructorInitializerSyntax constructorInitializerSyntax = SyntaxFactory.ConstructorInitializer(
                SyntaxKind.BaseConstructorInitializer,
                SyntaxFactory.Token(SyntaxKind.ColonToken),
                SyntaxFactory.Token(SyntaxKind.BaseKeyword),
                SyntaxFactory.ArgumentList());

            ConstructorDeclarationSyntax constructor =
                SyntaxFactory.ConstructorDeclaration(SyntaxFactory.Identifier(className))
                    .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                    .AddBodyStatements()
                    .WithParameterList(SyntaxFactory.ParseParameterList("(HttpRepositorySettings settings, IHttpClientFactory httpClientFactory, ISerializer<string> serializer)"))
                    .WithInitializer(constructorInitializerSyntax);

            return constructor;
        }

        private MethodDeclarationSyntax GenerateMethod(OperationContext operationContext)
        {
            string returnTypeName = GetReturnType(operationContext);
            string relativeUrl = GenerateRelativeUrl(operationContext);

            StatementSyntax syntax = GetMethodSyntax(operationContext, relativeUrl);

            var methodDeclaration = SyntaxFactory.MethodDeclaration(SyntaxFactory.ParseTypeName(returnTypeName), $"{operationContext.Name.ToCamelCase()}Async")
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.VirtualKeyword))
                .WithBody(SyntaxFactory.Block(syntax));

            if (operationContext.BodyParameter != null)
            {
                string bodyParameterType = operationContext.BodyParameter?.Type?.ToCamelCase();
                string bodyParameterName = operationContext.BodyParameter?.Name;

                methodDeclaration = methodDeclaration.AddParameterListParameters(
                    SyntaxFactory.Parameter(
                        SyntaxFactory.Identifier(bodyParameterName))
                        .WithType(SyntaxFactory.ParseTypeName(bodyParameterType)));
            }

            if (operationContext.PathParameters.Any())
            {
                foreach (Parameter parameter in operationContext.PathParameters)
                {
                    methodDeclaration = methodDeclaration.AddParameterListParameters(
                        SyntaxFactory.Parameter(SyntaxFactory.Identifier(parameter.Name)).WithType(SyntaxFactory.ParseTypeName(parameter.Type)));
                }
            }

            if (operationContext.QueryParameters.Any())
            {
                foreach (Parameter parameter in operationContext.QueryParameters)
                {
                    methodDeclaration = methodDeclaration.AddParameterListParameters(
                        SyntaxFactory.Parameter(SyntaxFactory.Identifier(parameter.Name)).WithType(SyntaxFactory.ParseTypeName(parameter.Type)));
                }
            }

            return methodDeclaration;
        }

        private string GetReturnType(OperationContext operationContext)
        {
            string returnTypeName = "Task";

            if (!String.IsNullOrEmpty(operationContext.ReturnTypeName))
            {
                returnTypeName = $"Task<{operationContext.ReturnTypeName.ToCamelCase()}>";
            }

            return returnTypeName;
        }

        private StatementSyntax GetMethodSyntax(OperationContext operationContext, string relativeUrl)
        {
            StatementSyntax syntax;

            string method = $"HttpMethod.{operationContext.Method.ToLower().ToCamelCase()}";
            string returnType = operationContext.ReturnTypeName?.ToCamelCase();
            string bodyParameterType = operationContext.BodyParameter?.Type?.ToCamelCase();
            string bodyParameterName = operationContext.BodyParameter?.Name;

            if (String.IsNullOrEmpty(returnType))
            {
                if (operationContext.BodyParameter != null)
                {
                    syntax = SyntaxFactory.ParseStatement($"return SendRequestAsync<{bodyParameterType}>($\"{relativeUrl}\", " +
                        $"{method}, {bodyParameterName});");
                }
                else
                {
                    syntax = SyntaxFactory.ParseStatement($"return SendRequestAsync($\"{relativeUrl}\", " +
                        $"{method});");
                }
            }
            else
            {
                if (operationContext.BodyParameter != null)
                {
                    syntax = SyntaxFactory.ParseStatement($"return SendRequestWithResultAsync<{bodyParameterType}, {returnType}>($\"{relativeUrl}\", " +
                        $"{method}, {bodyParameterName});");
                }
                else
                {
                    syntax = SyntaxFactory.ParseStatement($"return SendRequestWithResultAsync<{returnType}>($\"{relativeUrl}\", " +
                        $"{method});");
                }
            }

            return syntax;
        }

        private string GenerateRelativeUrl(OperationContext operationContext)
        {
            string relativeUrl = operationContext.RelativeUrl;

            if (operationContext.QueryParameters.Any())
            {
                relativeUrl = $"{relativeUrl}?{String.Join("&", operationContext.QueryParameters.Select(c => $"{c.Name}={{{c.Name}}}"))}";
            }

            return relativeUrl;
        }
    }
}