using System.Collections.Generic;
using System.IO;

using DZzzz.Swag.CodeGeneration.CSharp.Common;
using DZzzz.Swag.Generator.Core.Model;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DZzzz.Swag.CodeGeneration.CSharp
{
    public class CSharpLanguageModelGenerator
    {
        private readonly CSharpLanguageSettings configuration;
        private readonly CSharpTypeNameResolver typeNameResolver;

        public CSharpLanguageModelGenerator(CSharpLanguageSettings configuration, CSharpTypeNameResolver typeNameResolver)
        {
            this.configuration = configuration;
            this.typeNameResolver = typeNameResolver;
        }

        public void GenerateModelClasses(GenerationContext context)
        {
            foreach (KeyValuePair<string, DataModelContext> contextDataModel in context.DataModels)
            {
                GenerateModelClass(contextDataModel);
            }
        }

        private void GenerateModelClass(KeyValuePair<string, DataModelContext> contextDataModel)
        {
            try
            {
                string className = contextDataModel.Key.ToCamelCase();
                string fileName = $"{className}.cs";
                string fileLocation = Path.Combine(configuration.OutputFolder, $"{configuration.OutputProjectName}\\Model\\{fileName}");

                CompilationUnitSyntax compilationUnit = SyntaxFactory.CompilationUnit();
                compilationUnit = compilationUnit
                    .AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System")))
                    .AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System.Collections.Generic")))
                    .AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("Newtonsoft.Json")));

                var @namespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName($"{configuration.OutputProjectName}.Model")).NormalizeWhitespace();

                var classDeclaration = SyntaxFactory.ClassDeclaration(className)
                    .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));

                foreach (KeyValuePair<string, Parameter> valuePropety in contextDataModel.Value.Propeties)
                {
                    string typeName = typeNameResolver.ResolveType(valuePropety.Value);

                    if (valuePropety.Value.IsCollectionParameter)
                    {
                        typeName = $"List<{typeName}>";
                    }

                    ExpressionSyntax attributeSyntax = SyntaxFactory.ParseExpression($"\"{valuePropety.Value.Name}\"");

                    AttributeSyntax attribute = SyntaxFactory.Attribute(SyntaxFactory.IdentifierName("JsonProperty"))
                        .WithArgumentList(SyntaxFactory.AttributeArgumentList()
                            .WithOpenParenToken(SyntaxFactory.Token(SyntaxKind.OpenParenToken))
                            .WithCloseParenToken(SyntaxFactory.Token(SyntaxKind.CloseParenToken))
                            .AddArguments(SyntaxFactory.AttributeArgument(attributeSyntax)));

                    PropertyDeclarationSyntax property = SyntaxFactory.PropertyDeclaration(
                        SyntaxFactory.ParseTypeName(typeName), SyntaxFactory.Identifier(valuePropety.Value.Name.ToCamelCase()))
                            .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                            .AddAttributeLists(
                                SyntaxFactory.AttributeList()
                                .WithOpenBracketToken(SyntaxFactory.Token(SyntaxKind.OpenBracketToken))
                                .WithCloseBracketToken(SyntaxFactory.Token(SyntaxKind.CloseBracketToken))
                                .AddAttributes(attribute))
                            .AddAccessorListAccessors(
                                SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                                SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)));

                    classDeclaration = classDeclaration.AddMembers(property);
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
    }
}