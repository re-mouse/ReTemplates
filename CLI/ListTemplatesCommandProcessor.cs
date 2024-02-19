using ReTemplate.CLI.Consts;
using ReTemplate.Repository;

namespace ReTemplate.CLI;

public class ListTemplatesCommandProcessor : ICommandProcessor
{
    private TemplatesRepository _templatesRepository = new TemplatesRepository();

    public void Process(List<string> args)
    {
        if (args.Count > 0)
        {
            Console.WriteLine(Error.TooManyArgumentsError);
            return;
        }
    
        var templates = _templatesRepository.GetAllTemplateNames();
        foreach (var templateName in templates)
        {
            Console.WriteLine(templateName);
        }
    }
}