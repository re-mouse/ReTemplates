using ReDI;
using ReTemplate.Repository;

namespace ReTemplate.Commands;

public class RemoveTemplateFromRepositoryCommand
{
    [Inject] private TemplatesRepository _repository;

    public void RemoveFromRepository(string templateName)
    {
        _repository.Remove(templateName);
    }
}