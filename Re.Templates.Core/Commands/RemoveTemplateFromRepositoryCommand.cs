using Re.Templates.Repository;
using ReDI;

namespace Re.Templates.Commands;

public class RemoveTemplateFromRepositoryCommand
{
    [Inject] private TemplatesRepository _repository;

    public void RemoveFromRepository(string templateName)
    {
        _repository.Remove(templateName);
    }
}