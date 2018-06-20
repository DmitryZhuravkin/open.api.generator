namespace DZzzz.Swag.CodeGeneration.CSharp.Configuration
{
    public class CSharpLanguageSettings
    {
        public string OutputFolder { get; set; }

        public string OutputProjectName { get; set; }

        public CSharpModelSettings ModelSettings { get; set; }

        public CSharpOperationSettings OperationSettings { get; set; }
    }
}