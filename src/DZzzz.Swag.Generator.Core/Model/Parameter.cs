namespace DZzzz.Swag.Generator.Core.Model
{
    public class Parameter
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public string Format { get; set; }

        public bool IsCollectionParameter { get; set; }

        public bool Required { get; set; }
    }
}