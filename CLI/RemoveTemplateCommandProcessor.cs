using ReTemplate.CLI.Consts;
using ReTemplate.Repository;

namespace ReTemplate.CLI;

public class RemoveTemplateCommandProcessor : ICommandProcessor
{
    private TemplatesRepository _templatesRepository = new TemplatesRepository();

    public void Process(List<string> args)
    {
        if (args.Count > 1)
        {
            Console.WriteLine(Error.TooManyArgumentsError);
            return;
        }
        
        var pathInput = ".";
        if (args.Count == 1)
        {
            pathInput = args[0];
        }
        
        var templateName = PathUtils.ConvertInputToFullPath(pathInput);
        _templatesRepository.Remove(templateName);
    }
}