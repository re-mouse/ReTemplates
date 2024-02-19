namespace ReTemplate;

public class ProjectFile
{
    public string Name { get; }
    public string Text { get; }
    
    public ProjectFile(string name, string text)
    {
        Name = name;
        Text = text;
    }
}