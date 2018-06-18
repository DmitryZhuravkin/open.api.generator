using System.Threading.Tasks;

using DZzzz.Swag.Generator.Core.Model;

namespace DZzzz.Swag.Generator.Core.Interfaces
{
    public interface ISpecificationProvider
    {
        Task<GenerationContext> GetSpecificationAsync();
    }
}