namespace DZzzz.Swag.CodeGeneration.CSharp.Configuration
{
    public class CSharpOperationSettings
    {
        public string ClassBaseName { get; set; } = "Repository";

        public string ClassPrefixName { get; set; }

        public string FodlerName { get; set; } = "Repositories";

        public string GeneratedFolderName { get; set; } = "Generated";

        public string BaseFolderName { get; set; } = "Base";
    }
}