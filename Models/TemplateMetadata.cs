namespace ReTemplate;

public class TemplateMetadata
{
    public List<string> Definitions { get; } = new List<string>();
    public List<string> Placeholder { get; } = new List<string>();
    public List<TemplateArrayMetadata> Arrays { get; } = new List<TemplateArrayMetadata>();
}

public class TemplateArrayMetadata
{
    public string Name { get; }
    public List<string> Definitions { get; } = new List<string>();
    public List<string> Placeholder { get; } = new List<string>();
    public List<TemplateArrayMetadata> Arrays { get; } = new List<TemplateArrayMetadata>();
    
    public TemplateArrayMetadata(string name)
    {
        Name = name;
    }
}