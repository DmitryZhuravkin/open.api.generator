using System.Collections.Generic;

using DZzzz.Swag.Generator.Core.Model;

namespace DZzzz.Swag.Generator.Core.Interfaces
{
    public interface ILanguageCodeGenerator
    {
        void Generate(IList<OperationGroupContext> context);
    }
}