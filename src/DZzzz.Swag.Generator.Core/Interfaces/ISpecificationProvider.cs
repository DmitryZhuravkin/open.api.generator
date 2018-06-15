using System.Collections.Generic;

using DZzzz.Swag.Generator.Core.Model;

namespace DZzzz.Swag.Generator.Core.Interfaces
{
    public interface ISpecificationProvider
    {
        IList<OperationGroupContext> GetSpecification();
    }
}