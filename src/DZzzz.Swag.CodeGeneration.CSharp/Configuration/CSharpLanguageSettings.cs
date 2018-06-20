using System.IO;

namespace DZzzz.Swag.CodeGeneration.CSharp.Configuration
{
    public class CSharpLanguageSettings
    {
        public const string GeneratedKeyWord = "Generated";
        public const string BaseKeyWord = "Base";

        public string OutputFolder { get; set; } = "Test.Project";

        public string ProjectName { get; set; } = "Test.Project";

        // for model
        public string ModelFolderName { get; set; } = "Model";

        // for operations
        public string OperationClassBaseName { get; set; } = "Repository";

        public string OperationClassPrefixName { get; set; } = "Temp";

        public string OperationsFolderName { get; set; } = "Repositories";

        public string OperationsGeneratedFolderName { get; set; } = GeneratedKeyWord;

        public string OperationsBaseFolderName { get; set; } = BaseKeyWord;

        // derivative properties

        public string ModelFolderLocation => Path.Combine(OutputFolder, ModelFolderName);

        public string OperationsFolderLocation => Path.Combine(OutputFolder, OperationsFolderName);

        public string OperationsGeneratedFolderLocation => Path.Combine(OperationsFolderLocation, OperationsGeneratedFolderName);

        public string OperationsBaseFolderLocation => Path.Combine(OperationsFolderLocation, OperationsBaseFolderName);
    }
}