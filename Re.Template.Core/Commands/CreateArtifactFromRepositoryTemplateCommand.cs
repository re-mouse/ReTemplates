using Re.Template.IO;
using Re.Template.Repository;
using ReDI;

namespace Re.Template.Commands;

public class CreateArtifactFromRepositoryTemplateCommand
{
    [Inject] private TemplatesRepository _repository;
    [Inject] private ArtifactWriter _artifactWriter;
    [Inject] private ArtifactFactory _artifactFactory;

    public void Create(string templateName, string savingPath, bool canOverride)
    {
        var template = _repository.GetTemplateByName(templateName);
        var artifact = _artifactFactory.Create(template);
        _artifactWriter.Write(artifact, savingPath, canOverride);
    }
}