namespace DZzzz.Swag.Generator.Core.Configuration
{
    public class GeneratorSettings
    {
        //[JsonProperty("swaggerURL")]
        public string SwaggerUrl { get; set; }

        //[JsonProperty("swaggerFormat")]
        public string SwaggerFormat { get; set; }

        //[JsonProperty("swaggerVersion")]
        public string SwaggerVersion { get; set; }

        //[JsonProperty("outputFolder")]
        public string OutputFolder { get; set; }

        //[JsonProperty("outputSolutionName")]
        public string OutputSolutionName { get; set; }

        //[JsonProperty("language")]
        public string Language { get; set; }
    }
}