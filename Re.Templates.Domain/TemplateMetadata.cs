namespace Re.Templates;

public class TemplateMetadata
{
    public List<string> Definitions { get; } = new();
    public List<string> Placeholder { get; } = new();
    public List<TemplateArrayMetadata> Arrays { get; } = new();
}

public class TemplateArrayMetadata
{
    public string Name { get; }
    public List<string> Definitions { get; } = new();
    public List<string> Placeholder { get; } = new();
    public List<TemplateArrayMetadata> Arrays { get; } = new();
    
    public TemplateArrayMetadata(string name)
    {
        Name = name;
    }
}