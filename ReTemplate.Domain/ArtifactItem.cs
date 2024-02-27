namespace ReTemplate;

public class ArtifactItem
{
    public string Name { get; }
    public string Text { get; }
    
    public ArtifactItem(string name, string text)
    {
        Name = name;
        Text = text;
    }
}