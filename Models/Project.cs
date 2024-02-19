namespace ReTemplate;

public class Project
{
    public ProjectDirectory RootDirectory { get; }
    public string Path { get; }

    public Project(ProjectDirectory rootDirectory, string path)
    {
        RootDirectory = rootDirectory;
        Path = path;
    }
}