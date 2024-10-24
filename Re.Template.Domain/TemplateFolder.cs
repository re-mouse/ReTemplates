namespace Re.Template;

public class TemplateFolder
{
    public string Name { get; }
    public List<TemplateItem> Items { get; } = new();
    public List<TemplateFolder> Folders { get; } = new();

    public TemplateFolder(string name)
    {
        Name = name;
    }
}