using ReDI;
using ReTemplate.IO;

namespace ReTemplate.Commands;

public class CreateArtifactFromDiskTemplateCommand
{
    [Inject] private TemplateReader _templateReader;
    [Inject] private ArtifactWriter _artifactWriter;
    [Inject] private ArtifactFactory _artifactFactory;

    public void Create(string templatePath, string savingPath, bool canOverride)
    {
        var template = _templateReader.Read(templatePath);
        var artifact = _artifactFactory.Create(template);
        _artifactWriter.Write(artifact, savingPath, canOverride);
    }
}