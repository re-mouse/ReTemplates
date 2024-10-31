using Re.Templates.IO;
using Re.Templates.Repository;
using ReDI;

namespace Re.Templates.Commands;

public class SaveTemplateFromDiskInRepositoryCommand
{
    [Inject] private TemplateReader _reader;
    [Inject] private TemplateValidator _validator;
    [Inject] private TemplatesRepository _repository;

    public void SaveFromDisk(string templateDirectory)
    {
        var template = _reader.Read(templateDirectory);
        _validator.Validate(template.Root);
        
        _repository.Save(template);
    }
}