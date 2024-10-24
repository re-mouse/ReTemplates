namespace Re.Template;

public class Artifact
{
    public ArtifactFolder Root { get; }

    public Artifact(ArtifactFolder root)
    {
        Root = root;
    }
}