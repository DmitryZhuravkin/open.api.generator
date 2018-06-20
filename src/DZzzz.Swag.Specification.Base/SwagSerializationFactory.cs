using System;

using DZzzz.Swag.Generator.Core.Interfaces;
using DZzzz.Swag.Generator.Infrastructure.Serialization;

namespace DZzzz.Swag.Specification.Base
{
    public class SwagSerializationFactory
    {
        public static ISerializer<string> GetSerilizer(SwagFormat format)
        {
            switch (format)
            {
                case SwagFormat.Json:
                    return new NewtonJsonSerializer();
                case SwagFormat.Yaml:
                    throw new ArgumentOutOfRangeException(nameof(format), format, null);
                default:
                    throw new ArgumentOutOfRangeException(nameof(format), format, null);
            }
        }
    }
}