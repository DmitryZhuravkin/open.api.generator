namespace DZzzz.Swag.Generator.Core.Model
{
    public class ParameterContext : TypeContext
    {
        public const string DefaultParameterName = "request";

        public string Name { get; set; }

        public bool Required { get; set; }
    }
}