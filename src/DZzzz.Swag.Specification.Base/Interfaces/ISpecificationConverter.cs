using DZzzz.Swag.Generator.Core.Model;

namespace DZzzz.Swag.Specification.Base.Interfaces
{
    public interface ISpecificationConverter<in T>
    {
        GenerationContext Convert(T model);
    }
}