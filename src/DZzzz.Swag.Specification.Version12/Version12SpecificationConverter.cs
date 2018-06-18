using DZzzz.Swag.Generator.Core.Model;
using DZzzz.Swag.Specification.Base.Interfaces;

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
                        Method = operation.Method.ToString()
                    };

                    operationGroupContext.Operations.Add(operationContext);
                }
            }
        }
    }
}