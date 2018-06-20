using System;
using System.IO;
using System.Linq;

using DZzzz.Swag.CodeGeneration.CSharp.Common;
using DZzzz.Swag.CodeGeneration.CSharp.Configuration;
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
        private readonly CSharpTypeNameResolver typeNameResolver;

        public CSharpLanguageCodeGenerator(CSharpLanguageSettings configuration)
        {
            this.configuration = configuration;

            typeNameResolver = new CSharpTypeNameResolver();
            modelGenerator = new CSharpLanguageModelGenerator(configuration, typeNameResolver);
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



            try
            {

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
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

                //ConstructorDeclarationSyntax constructor = GenerateConstructor(className);

                ClassDeclarationSyntax classDeclaration = SyntaxFactory.ClassDeclaration(className)
                    .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                    .AddBaseListTypes(SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName($"{configuration.FileNamePrefix}RepositoryBase")));
                //.AddMembers(constructor);

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

        //private ConstructorDeclarationSyntax GenerateConstructor(string className)
        //{
        //    ConstructorInitializerSyntax constructorInitializerSyntax = SyntaxFactory.ConstructorInitializer(
        //        SyntaxKind.BaseConstructorInitializer,
        //        SyntaxFactory.Token(SyntaxKind.ColonToken),
        //        SyntaxFactory.Token(SyntaxKind.BaseKeyword),
        //        SyntaxFactory.ArgumentList());

        //    ConstructorDeclarationSyntax constructor =
        //        SyntaxFactory.ConstructorDeclaration(SyntaxFactory.Identifier(className))
        //            .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
        //            .AddBodyStatements()
        //            .WithParameterList(SyntaxFactory.ParseParameterList("(HttpRepositorySettings settings, IHttpClientFactory httpClientFactory, ISerializer<string> serializer)"))
        //            .WithInitializer(constructorInitializerSyntax);

        //    return constructor;
        //}

        private MethodDeclarationSyntax GenerateMethod(OperationContext operationContext)
        {
            string methodReturnType = GetMethodReturnType(operationContext);
            string relativeUrl = GenerateRelativeUrl(operationContext);

            StatementSyntax syntax = GetMethodSyntax(operationContext, relativeUrl);

            var methodDeclaration = SyntaxFactory.MethodDeclaration(SyntaxFactory.ParseTypeName(methodReturnType), $"{operationContext.Name.ToCamelCase()}Async")
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.VirtualKeyword))
                .WithBody(SyntaxFactory.Block(syntax));

            if (operationContext.BodyParameter != null)
            {
                string bodyParameterType = operationContext.BodyParameter?.Type?.ToCamelCase();

                methodDeclaration = methodDeclaration.AddParameterListParameters(
                    SyntaxFactory.Parameter(
                        SyntaxFactory.Identifier(ParameterContext.DefaultParameterName))
                        .WithType(SyntaxFactory.ParseTypeName(bodyParameterType)));
            }

            if (operationContext.PathParameters.Any())
            {
                foreach (ParameterContext parameter in operationContext.PathParameters)
                {
                    string typeName = typeNameResolver.ResolveType(parameter);
                    methodDeclaration = methodDeclaration.AddParameterListParameters(
                        SyntaxFactory.Parameter(SyntaxFactory.Identifier(parameter.Name)).WithType(SyntaxFactory.ParseTypeName(typeName)));
                }
            }

            if (operationContext.QueryParameters.Any())
            {
                foreach (ParameterContext parameter in operationContext.QueryParameters)
                {
                    string typeName = typeNameResolver.ResolveType(parameter);
                    methodDeclaration = methodDeclaration.AddParameterListParameters(
                        SyntaxFactory.Parameter(SyntaxFactory.Identifier(parameter.Name)).WithType(SyntaxFactory.ParseTypeName(typeName)));
                }
            }

            return methodDeclaration;
        }

        private string GetMethodReturnType(OperationContext operationContext)
        {
            string returnTypeName = "Task";
            string paramReturnType = typeNameResolver.ResolveType(operationContext);

            if (!String.IsNullOrEmpty(paramReturnType))
            {
                returnTypeName = $"Task<{paramReturnType}>";
            }

            return returnTypeName;
        }

        private StatementSyntax GetMethodSyntax(OperationContext operationContext, string relativeUrl)
        {
            StatementSyntax syntax;

            string method = $"HttpMethod.{operationContext.Method.ToLower().ToCamelCase()}";
            string returnType = typeNameResolver.ResolveType(operationContext);

            string bodyParameterType = typeNameResolver.ResolveType(operationContext.BodyParameter);

            if (String.IsNullOrEmpty(returnType))
            {
                if (operationContext.BodyParameter != null)
                {
                    syntax = SyntaxFactory.ParseStatement($"return SendRequestAsync<{bodyParameterType}>($\"{relativeUrl}\", " +
                        $"{method}, {ParameterContext.DefaultParameterName});");
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
                        $"{method}, {ParameterContext.DefaultParameterName});");
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