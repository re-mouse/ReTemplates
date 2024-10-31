using System.CommandLine;
using Re.Templates;
using Re.Templates.Commands;
using ReDI;

namespace Re.Templates.CLI;

public class RemoveCliCommand : ICliCommand
{
    [Inject] private RemoveTemplateFromRepositoryCommand _command;

    public Command AsCLICommand()
    {
        var templateNameArgument = new Argument<string>("TemplateName", "Name of template that has to be removed");
        var command = new Command("remove", "Remove template from repository");
        command.AddArgument(templateNameArgument);
        command.SetHandler(RunCommand, templateNameArgument);
        return command;
    }

    private void RunCommand(string templateName)
    {

        try
        {
            _command.RemoveFromRepository(templateName);
            return;
        }
        catch (TemplateNotFoundException)
        {
            Console.WriteLine($"Template with name {templateName} not found in repository. Cannot remove");
        }
        Program.StatusCode = 1;
    }
}