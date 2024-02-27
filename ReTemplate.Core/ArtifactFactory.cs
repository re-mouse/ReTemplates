using ReDI;
using ReTemplate;

public class ArtifactFactory
{
    [Inject] private TemplateFormatter _formatter;
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
            var formattedText = _formatter.Format(file.Text, args);
            folder.Items.Add(new ArtifactItem(file.Name, formattedText));
        }

        foreach (var templateSubdirectory in templateFolder.Folders)
        {
            folder.Folders.Add(CreateArtifactFolder(templateSubdirectory, args));
        }

        return folder;
    }
}