using System.CommandLine;
using Re.Template.Repository;
using ReDI;

namespace Re.Template.CLI;

public class ListCliCommand : ICliCommand
{
    [Inject] private TemplatesRepository _repository;

    public Command AsCLICommand()
    {
        var command = new Command("list", "List all templates stored in repository");
        command.SetHandler(RunCommand);
        return command;
    }
    
    private void RunCommand()
    {
        var templates = _repository.GetAllTemplateNames();

        if (templates.Count == 0)
        {
            Console.WriteLine("Repository doesn't contain any templates. Add them by 'save' command. See --help");
        }
        foreach (var templateName in templates)
        {
            Console.WriteLine(templateName);
        }
    }
}