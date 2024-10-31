using Re.Templates.IO;
using ReDI;

namespace Re.Templates.Commands;

public class ValidateTemplateDirectoryCommand
{
    [Inject] private TemplateValidator _validator;
    [Inject] private TemplateReader _templateReader;
    
    public void Validate(string templatepath)
    {
        var template = _templateReader.Read(templatepath);
        _validator.Validate(template.Root);
    }
}