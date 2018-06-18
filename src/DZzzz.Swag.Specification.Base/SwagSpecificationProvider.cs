using System.Threading.Tasks;

using DZzzz.Swag.Generator.Core.Model;
using DZzzz.Swag.Generator.Core.Interfaces;
using DZzzz.Swag.Specification.Base.Interfaces;
using DZzzz.Swag.Generator.Infrastructure.Http;
using DZzzz.Swag.Generator.Infrastructure.Serialization;

namespace DZzzz.Swag.Specification.Base
{
    public class SwagSpecificationProvider<TModel> : ISpecificationProvider
    {
        protected ISpecificationConverter<TModel> Converter { get; }

        protected SwagSpecificationContext Context { get; }

        protected ICommunicationService CommunicationService { get; }

        public SwagSpecificationProvider(ISpecificationConverter<TModel> converter, SwagSpecificationContext context)
        {
            Converter = converter;
            Context = context;

            CommunicationService = new HttpCommunicationService(new DefaultHttpClientFactory(), new NewtonJsonSerializer());
        }

        public virtual async Task<GenerationContext> GetSpecificationAsync()
        {
            TModel model = await CommunicationService.SendRequestAsync<TModel>(Context.Url);

            // potentially new need few extra calls to find area

            return Converter.Convert(model);
        }
    }
}
