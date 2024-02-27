namespace ReTemplate;

public class TemplateConfiguration
{
    public string Name { get; set; }
    public Dictionary<string, string> Prompts { get; set; } = new Dictionary<string, string>() {{"DefaultCondition", "Enable default?"}, {"DefaultPlaceholder", "What is default placeholder?"}};
    public Dictionary<string, string> DefaultValues { get; set; } = new Dictionary<string, string>() {{"DefaultCondition", "Yes"}, {"DefaultPlaceholder", "Default"}};
    public List<string> ExcludeFiles { get; set; } = new List<string>() {"template.yaml"};
    public List<string> ExcludeDirectories { get; set; } = new List<string>() {".git", ".DS_Store" };
    
    public TemplateConfiguration() {}
    public TemplateConfiguration(string name) { Name = name; }
}