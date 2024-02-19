using ReTemplate.CLI.Consts;
using ReTemplate.Repository;

namespace ReTemplate.CLI;

public class SaveTemplateCommandProcessor : ICommandProcessor
{
    private readonly TemplatesRepository _templatesRepository = new TemplatesRepository();
    
    public void Process(List<string> args)
    {
        if (args.Count == 0)
        {
            Console.WriteLine(TemplatesCommandError.EmptySaveCommand);
            return;
        }
        
        if (args.Count > 1)
        {
            Console.WriteLine(Error.TooManyArgumentsError);
            return;
        }

        var pathInput = args[0];

        var path = PathUtils.ConvertInputToFullPath(pathInput);
        _templatesRepository.Save(path);
    }
}