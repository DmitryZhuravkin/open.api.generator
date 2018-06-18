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

            ConvertDataModel();
            ConvertOperations(model, operationGroupContext);

            context.Groups.Add(operationGroupContext);

            return context;
        }

        private void ConvertDataModel()
        {

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
                            operationContext.BodyParameter = new BodyParameter
                            {
                                BodyParameterName = parameter.Name,
                                BodyParameterType = parameter.Type
                            };
                        }
                        else if (parameter.ParamType == ParameterObjectParamType.Path)
                        {
                            operationContext.PathParameters.Add(new PathParameter
                            {
                                Name = parameter.Name,
                                Type = parameter.Type,
                                Format = parameter.Format
                            });
                        }
                        else if (parameter.ParamType == ParameterObjectParamType.Query)
                        {
                            operationContext.QueryParameters.Add(new QueryParameter
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