namespace ReTemplate;

public class ProjectDirectory
{
    public string Name { get; }
    public List<ProjectFile> Files { get; } = new List<ProjectFile>();
    public List<ProjectDirectory> Directories { get; } = new List<ProjectDirectory>();
    
    public ProjectDirectory(string name) { Name = name; }
}