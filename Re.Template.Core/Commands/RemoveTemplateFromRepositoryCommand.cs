using Re.Template.Repository;
using ReDI;

namespace Re.Template.Commands;

public class RemoveTemplateFromRepositoryCommand
{
    [Inject] private TemplatesRepository _repository;

    public void RemoveFromRepository(string templateName)
    {
        _repository.Remove(templateName);
    }
}