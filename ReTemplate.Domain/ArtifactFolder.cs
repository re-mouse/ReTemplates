namespace ReTemplate;

public class ArtifactFolder
{
    public string Name { get; }
    public List<ArtifactItem> Items { get; } = new List<ArtifactItem>();
    public List<ArtifactFolder> Folders { get; } = new List<ArtifactFolder>();
    
    public ArtifactFolder(string name) { Name = name; }
}