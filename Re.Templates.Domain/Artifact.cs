namespace Re.Templates;

public class Artifact
{
    public ArtifactFolder Root { get; }

    public Artifact(ArtifactFolder root)
    {
        Root = root;
    }
}