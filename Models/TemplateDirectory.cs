namespace ReTemplate;

public class TemplateDirectory
{
    public string Name { get; }
    public List<TemplateFile> Files { get; } = new List<TemplateFile>();
    public List<TemplateDirectory> Directories { get; } = new List<TemplateDirectory>();

    public TemplateDirectory(string name)
    {
        Name = name;
    }
}