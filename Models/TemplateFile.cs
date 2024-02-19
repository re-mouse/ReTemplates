namespace ReTemplate;

public class TemplateFile
{
    public string Name { get; }
    public string Text { get; }
    
    public TemplateFile(string name, string text)
    {
        Name = name;
        Text = text;
    }
}