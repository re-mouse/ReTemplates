using ReDI;
using Re.Template;

public class ArtifactFactory
{
    [Inject] private TemplateDirectoryFormatter _directoryFormatter;
    [Inject] private TemplateFileFormatter _fileFormatter;
    [Inject] private TemplateMetadataFactory _metadataFactory;
    [Inject] private TemplateFormatArgsFactory _argsFactory;

    public Artifact Create(Template template)
    {
        var args = _argsFactory.Create(template);
        var artifactRootFolder = _directoryFormatter.Format(template.Root, args);

        return new Artifact(artifactRootFolder);
    }
}