using ReDI;
using Re.Template;

public class ArtifactFactory
{
    [Inject] private TemplateFileFormatter _fileFormatter;
    [Inject] private TemplateMetadataFactory _metadataFactory;
    [Inject] private TemplateFormatArgsFactory _argsFactory;

    public Artifact Create(Template template)
    {
        var args = _argsFactory.Create(template);
        var artifactRootFolder = CreateArtifactFolder(template.Root, args);
        return new Artifact(artifactRootFolder);
    }
    
    private ArtifactFolder CreateArtifactFolder(TemplateFolder templateFolder, TemplateFormatArgs args)
    {
        var folder = new ArtifactFolder(templateFolder.Name);
        foreach (var file in templateFolder.Items)
        {
            var artifacts = _fileFormatter.Format(file, args);

            if (artifacts.Count > 0)
            {
                folder.Items.AddRange(artifacts);
            }
        }

        foreach (var templateSubdirectory in templateFolder.Folders)
        {
            folder.Folders.Add(CreateArtifactFolder(templateSubdirectory, args));
        }

        return folder;
    }
}