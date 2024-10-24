namespace Re.Template;

public class TemplateConfiguration
{
    public string Name { get; set; }
    public Dictionary<string, string> Prompts { get; set; } = new() {{"DefaultCondition", "Enable default?"}, {"DefaultPlaceholder", "What is default placeholder?"}};
    public Dictionary<string, string> DefaultValues { get; set; } = new() {{"DefaultCondition", "Yes"}, {"DefaultPlaceholder", "Default"}};
    public List<string> ExcludeFiles { get; set; } = new() {"template.yaml"};
    public List<string> ExcludeDirectories { get; set; } = new() {".git", ".DS_Store" };
    
    public TemplateConfiguration() {}
    public TemplateConfiguration(string name) { Name = name; }
}