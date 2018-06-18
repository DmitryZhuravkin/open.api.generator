using System;
using System.Threading.Tasks;

using DZzzz.Swag.Generator.Core.Model;
using DZzzz.Swag.Specification.Base;

namespace DZzzz.Swag.Specification.Version12
{
    // NOTE: we override base behavior because of potentiall we need to do few additional calls in cased of empty resourcePath
    public class Version12SpecificationProvider : SwagSpecificationProvider<Json>
    {
        public Version12SpecificationProvider(SwagSpecificationContext context)
            : base(new Version12SpecificationConverter(), context)
        {
        }

        public override async Task<GenerationContext> GetSpecificationAsync()
        {
            GenerationContext commonContext = new GenerationContext();

            Uri uri = new Uri(Context.Url);

            Json model = await CommunicationService.SendRequestAsync<Json>(Context.Url);

            if (model != null)
            {
                if (String.IsNullOrEmpty(model.ResourcePath))
                {
                    foreach (ApiObject apiObject in model.Apis)
                    {
                        // TODO: refactor this
                        string relativeUrl = apiObject.Path.Replace("{format}", Context.Format);
                        string apiAreaUrl = $"http://{uri.Host}:{uri.Port}{model.BasePath}{relativeUrl}";

                        Json innerModel = await CommunicationService.SendRequestAsync<Json>(apiAreaUrl);

                        if (innerModel != null)
                        {
                            GenerationContext childContext = Converter.Convert(innerModel);

                            MergeIntoExistingContext(commonContext, childContext);
                        }
                    }
                }
                else
                {
                    commonContext = Converter.Convert(model);
                }
            }

            return commonContext;
        }

        private void MergeIntoExistingContext(GenerationContext existing, GenerationContext child)
        {
            foreach (OperationGroupContext operationGroupContext in child.Groups)
            {
                existing.Groups.Add(operationGroupContext);
            }
        }
    }
}