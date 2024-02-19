namespace ReTemplate;

public class TemplateConfiguration
{
    public string Name { get; }
    public Dictionary<string, string> Prompts { get; } = new Dictionary<string, string>();
    public Dictionary<string, string> DefaultValues { get; } = new Dictionary<string, string>();
    public List<string> ExcludeFiles { get; } = new List<string>();
    public List<string> ExcludeDirectories { get; } = new List<string>();
    
    public TemplateConfiguration(string name) { Name = name; }
}