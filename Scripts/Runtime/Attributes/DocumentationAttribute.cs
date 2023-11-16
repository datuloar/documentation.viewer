namespace datuloar.documentation
{
    [System.AttributeUsage(System.AttributeTargets.Class 
        | System.AttributeTargets.Enum 
        | System.AttributeTargets.Struct 
        | System.AttributeTargets.Interface)]
    public class DocumentationAttribute : System.Attribute
    {
        public DocumentationCatalogType DocType { get; }
        public string Text { get; }

        public DocumentationAttribute(DocumentationCatalogType type, string text)
        {
            DocType = type;
            Text = text;
        }
    }
}