using ReTemplate.CLI.Consts;
using ReTemplate.Repository;

namespace ReTemplate.CLI;

public class InitTemplateCommandProcessor : ICommandProcessor
{
    private TemplatesRepository _templatesRepository = new TemplatesRepository();
    
    public void Process(List<string> args)
    {
        if (args.Count == 0)
        {
            Console.WriteLine(TemplatesCommandError.EmptyInitCommand);
            return;
        }
        if (args.Count > 2)
        {
            Console.WriteLine(Error.TooManyArgumentsError);
            return;
        }
        
        var templateName = args[0];
        
        var pathInput = ".";
        if (args.Count > 1)
        {
            pathInput = args[1];
        }
        
        var path = PathUtils.ConvertInputToFullPath(pathInput);
        _templatesRepository.Init(path, templateName);
    }
}