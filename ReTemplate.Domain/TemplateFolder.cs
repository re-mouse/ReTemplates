namespace ReTemplate;

public class TemplateFolder
{
    public string Name { get; }
    public List<TemplateItem> Items { get; } = new List<TemplateItem>();
    public List<TemplateFolder> Folders { get; } = new List<TemplateFolder>();

    public TemplateFolder(string name)
    {
        Name = name;
    }
}