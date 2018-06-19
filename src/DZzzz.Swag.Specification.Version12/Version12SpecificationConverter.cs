using System;
using System.Collections.Generic;

using DZzzz.Swag.Generator.Core.Model;
using DZzzz.Swag.Specification.Base.Interfaces;
using DZzzz.Swag.Specification.Version12.Common;

namespace DZzzz.Swag.Specification.Version12
{
    public class Version12SpecificationConverter : ISpecificationConverter<Json>
    {
        public GenerationContext Convert(Json model)
        {
            GenerationContext context = new GenerationContext();
            OperationGroupContext operationGroupContext = new OperationGroupContext { Name = model.ResourcePath };
            // TODO: init name

            ConvertDataModel(model, context);
            ConvertOperations(model, operationGroupContext);

            context.Groups.Add(operationGroupContext);

            return context;
        }

        private void ConvertDataModel(Json model, GenerationContext context)
        {
            foreach (KeyValuePair<string, ModelsObject> modelsObject in model.Models)
            {
                DataModelContext dataModelContext = new DataModelContext
                {
                    ID = modelsObject.Value.Id
                };

                foreach (KeyValuePair<string, PropertyObject> propertyObject in modelsObject.Value.Properties)
                {
                    Parameter parameter = new Parameter
                    {
                        Name = propertyObject.Key,
                        Format = propertyObject.Value.Format,
                        IsCollectionParameter = String.Compare(propertyObject.Value.Type, "array", StringComparison.OrdinalIgnoreCase) == 0
                    };

                    if (parameter.IsCollectionParameter)
                    {
                        // TODO: change this
                        parameter.Type = propertyObject.Value.Type + "TT";
                    }
                    else
                    {
                        parameter.Type = propertyObject.Value.Type;
                    }

                    dataModelContext.Propeties.Add(propertyObject.Key, parameter);
                }

                context.DataModels.Add(modelsObject.Key, dataModelContext);
            }
        }

        private void ConvertOperations(Json model, OperationGroupContext operationGroupContext)
        {
            foreach (ApiObject apiObject in model.Apis)
            {
                string operationPath = apiObject.Path;

                foreach (OperationObject operation in apiObject.Operations)
                {
                    OperationContext operationContext = new OperationContext
                    {
                        RelativeUrl = $"{model.BasePath}{operationPath}",
                        Method = operation.Method.ToHttpMethod(),
                        Name = operation.Nickname,
                        ReturnTypeName = operation.Type
                    };

                    foreach (ParameterObject parameter in operation.Parameters)
                    {
                        // TODO: parse all others
                        if (parameter.ParamType == ParameterObjectParamType.Body)
                        {
                            operationContext.BodyParameter = new Parameter
                            {
                                Type = parameter.Type
                            };
                        }
                        else if (parameter.ParamType == ParameterObjectParamType.Path)
                        {
                            operationContext.PathParameters.Add(new Parameter
                            {
                                Name = parameter.Name,
                                Type = parameter.Type,
                                Format = parameter.Format
                            });
                        }
                        else if (parameter.ParamType == ParameterObjectParamType.Query)
                        {
                            operationContext.QueryParameters.Add(new Parameter
                            {
                                Name = parameter.Name,
                                Type = parameter.Type
                            });
                        }
                    }

                    operationGroupContext.Operations.Add(operationContext);
                }
            }
        }
    }
}