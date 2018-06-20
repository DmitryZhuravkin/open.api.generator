using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DZzzz.Swag.Generator.Core.Interfaces;
using DZzzz.Swag.Generator.Core.Model;
using DZzzz.Swag.Generator.Infrastructure.Http;
using DZzzz.Swag.Specification.Base;
using DZzzz.Swag.Specification.Version12.Model.Declaration;
using DZzzz.Swag.Specification.Version12.Model.Listing;

using ParamType = DZzzz.Swag.Specification.Version12.Model.Declaration.ParamType;

namespace DZzzz.Swag.Specification.Version12
{
    public class Version12SpecificationProvider : ISpecificationProvider
    {
        private readonly SwagSpecificationContext context;
        private readonly ICommunicationService communicationService;

        public Version12SpecificationProvider(SwagSpecificationContext context)
        {
            this.context = context;

            ISerializer<string> serializer = SwagSerializationFactory.GetSerilizer(context.Format);

            communicationService = new HttpCommunicationService(new DefaultHttpClientFactory(), serializer);
        }

        public async Task<GenerationContext> GetSpecificationAsync()
        {
            GenerationContext dataContext = new GenerationContext();

            ResourceListing resourceListing = await communicationService.SendRequestAsync<ResourceListing>(context.Url);

            foreach (Resource resource in resourceListing.Apis)
            {
                await ProcessResource(resource, dataContext);
            }

            return dataContext;
        }

        private async Task ProcessResource(Resource resource, GenerationContext commonContext)
        {
            Uri uri = new Uri(context.Url);

            // TODO: refactor this
            string relativeUrl = resource.Path.Replace("{format}", context.Format.ToString().ToLower()).Replace("/", "");
            string apiDeclarationUrl = $"http://{uri.Authority}{String.Join("", uri.Segments.Take(uri.Segments.Length - 1))}{relativeUrl}";

            ApiDeclaration apiDeclaration = await communicationService.SendRequestAsync<ApiDeclaration>(apiDeclarationUrl);

            Task processOperationTask = Task.Run(() => ProcessOperations(apiDeclaration, commonContext));
            Task processModelTask = Task.Run(() => ProcessModels(apiDeclaration, commonContext));

            await Task.WhenAll(processOperationTask, processModelTask);
        }

        private void ProcessModels(ApiDeclaration apiDeclaration, GenerationContext dataContext)
        {
            foreach (KeyValuePair<string, ModelObject> modelObject in apiDeclaration.Models)
            {
                if (!dataContext.DataModels.ContainsKey(modelObject.Key))
                {
                    DataModelContext dataModelContext = new DataModelContext
                    {
                        ID = modelObject.Value.ID
                    };

                    dataModelContext.SubTypes.AddRange(modelObject.Value.SubTypes);

                    foreach (KeyValuePair<string, PropertyObject> propertyObject in modelObject.Value.Properties)
                    {
                        Parameter parameter = new Parameter { Name = propertyObject.Key };
                        InitParameterFromDataTypeObject(parameter, propertyObject.Value);

                        dataModelContext.Propeties.Add(propertyObject.Key, parameter);
                    }

                    dataContext.DataModels.Add(modelObject.Key, dataModelContext);
                }
            }
        }

        private void ProcessOperations(ApiDeclaration apiDeclaration, GenerationContext dataContext)
        {
            OperationGroupContext operationGroupContext = new OperationGroupContext { Name = apiDeclaration.ResourcePath };

            foreach (ApiObject apiObject in apiDeclaration.Apis)
            {
                string operationPath = apiObject.Path;

                foreach (OperationObject operation in apiObject.Operations)
                {
                    OperationContext operationContext = new OperationContext
                    {
                        RelativeUrl = $"{apiDeclaration.BasePath}{operationPath}",
                        Method = operation.Method.ToString(),
                        Name = operation.NickName,
                        ReturnTypeName = operation.Type
                    };

                    foreach (ParameterObject parameterObject in operation.Parameters)
                    {
                        Parameter parameter = new Parameter
                        {
                            Name = parameterObject.Name
                        };

                        InitParameterFromDataTypeObject(parameter, parameterObject);

                        switch (parameterObject.ParamType)
                        {
                            case ParamType.Path:
                                operationContext.PathParameters.Add(parameter);
                                break;
                            case ParamType.Query:
                                operationContext.QueryParameters.Add(parameter);
                                break;
                            case ParamType.Body:
                                operationContext.BodyParameter = parameter;
                                break;
                            case ParamType.Header:
                            case ParamType.Form:
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }

                    operationGroupContext.Operations.Add(operationContext);
                }
            }

            dataContext.Groups.Add(operationGroupContext);
        }

        private void InitParameterFromDataTypeObject(Parameter parameter, DataTypeObject dataTypeObject)
        {
            parameter.IsCollectionParameter = String.Compare(dataTypeObject.Type, "array", StringComparison.OrdinalIgnoreCase) == 0;

            if (parameter.IsCollectionParameter)
            {
                if (String.IsNullOrEmpty(dataTypeObject.Items.ReferenceTypeID))
                {
                    parameter.Type = dataTypeObject.Items.Type;
                    parameter.Format = dataTypeObject.Items.Format;
                }
                else
                {
                    parameter.Type = dataTypeObject.Items.ReferenceTypeID;
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(dataTypeObject.Type))
                {
                    parameter.Type = dataTypeObject.Type;
                    parameter.Format = dataTypeObject.Format;
                }
                else
                {
                    // TODO: process $ref
                }
            }
        }
    }
}