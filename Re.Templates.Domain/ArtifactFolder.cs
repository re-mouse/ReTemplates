namespace Re.Templates;

public class ArtifactFolder
{
    public string Name { get; }
    public List<ArtifactItem> Items { get; } = new();
    public List<ArtifactFolder> Folders { get; } = new();
    
    public ArtifactFolder(string name) { Name = name; }
}