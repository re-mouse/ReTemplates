namespace Re.Templates;

public class TemplateItem
{
    public string Name { get; }
    public string Text { get; }
    
    public TemplateItem(string name, string text)
    {
        Name = name;
        Text = text;
    }
}