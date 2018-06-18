
using System;
using System.IO;

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

        public CSharpLanguageCodeGenerator(CSharpLanguageSettings configuration)
        {
            this.configuration = configuration;
        }

        public void Generate(GenerationContext context)
        {
            foreach (OperationGroupContext operationGroupContext in context.Groups)
            {
                GenerateRepositoryClass(operationGroupContext);
            }

        }

        private void GenerateModelClasses()
        {
        }

        private void GenerateRepositoryClass(OperationGroupContext operationGroupContext)
        {
            try
            {
                string className = $"{configuration.RepositoryFileNamePrefix}{operationGroupContext.Name.NormalizeName()}Repository";
                string fileName = $"{className}.cs";
                string fileLocation = Path.Combine(configuration.OutputFolder, fileName);

                CompilationUnitSyntax compilationUnit = SyntaxFactory.CompilationUnit();
                compilationUnit = compilationUnit
                    .AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System.Net.Http")))
                    .AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System.Threading.Tasks")))
                    .AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName($"{configuration.OutputProjectName}.Models")));

                var @namespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName($"{configuration.OutputProjectName}.Repositories")).NormalizeWhitespace();

                var classDeclaration = SyntaxFactory.ClassDeclaration(className)
                    .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                    .AddBaseListTypes(SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName($"{configuration.RepositoryFileNamePrefix}RepositoryBase")));

                classDeclaration = classDeclaration.AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));

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

        private MethodDeclarationSyntax GenerateMethod(OperationContext operationContext)
        {
            string returnTypeName = GetReturnType(operationContext);
            StatementSyntax syntax = GetMethodSyntax(operationContext);

            var methodDeclaration = SyntaxFactory.MethodDeclaration(SyntaxFactory.ParseTypeName(returnTypeName), $"{operationContext.Name.NormalizeName()}Async")
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .WithBody(SyntaxFactory.Block(syntax));




            return methodDeclaration;
        }

        private string GetReturnType(OperationContext operationContext)
        {
            string returnTypeName = "Task";

            if (!String.IsNullOrEmpty(operationContext.ReturnTypeName.NormalizeName()))
            {
                returnTypeName = $"Task<{operationContext.ReturnTypeName.NormalizeName()}>";
            }

            return returnTypeName;
        }

        private StatementSyntax GetMethodSyntax(OperationContext operationContext)
        {
            // return SendRequestAsync<AppAuthRequest, AppAuthResponse>("auth/application/login", HttpMethod.Post, request);
            // return SendRequestAsync<AppAuthResponse>("auth/application/login", HttpMethod.Post, request);
            StatementSyntax syntax = null;

            if (String.IsNullOrEmpty(operationContext.ReturnTypeName))
            {
                if (operationContext.BodyParameter != null)
                {
                    syntax = SyntaxFactory.ParseStatement($"return SendRequestAsync<{operationContext.BodyParameter.BodyParameterType}>(\"{}\", " +
                                                          $"{operationContext.Method}, {operationContext.BodyParameter.BodyParameterName});");
                }
                else
                {
                    syntax = SyntaxFactory.ParseStatement($"return SendRequestAsync(\"{}\", " +
                                                          $"{operationContext.Method});");
                }
            }
            else
            {
                if (operationContext.BodyParameter != null)
                {
                    syntax = SyntaxFactory.ParseStatement($"return SendRequestAsync<{operationContext.ReturnTypeName}, {operationContext.BodyParameter.BodyParameterType}>(\"{}\", " +
                                                          $"{operationContext.Method}, {operationContext.BodyParameter.BodyParameterName});");
                }
                else
                {
                    syntax = SyntaxFactory.ParseStatement($"return SendRequestAsync<{operationContext.ReturnTypeName}>(\"{}\", " +
                                                          $"{operationContext.Method});");
                }
            }

            return syntax;
        }
    }
}