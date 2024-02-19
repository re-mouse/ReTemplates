using ReTemplate;
using ReTemplate.CLI;
using ReTemplate.CLI.Consts;

public static class Program
{
    public static int Main(string[] args)
    {
        var debug = Console.ReadLine();
        var debugArgs = debug.Split(' ');
        var argsList = debugArgs.ToList();
        
        if (argsList.Count == 0)
        {
            Console.WriteLine(Error.EmptyArguments);
            return 0;
        }

        var command = argsList[0].ToLower();
        ICommandProcessor processor = command switch
        {
            "create" => new CreateCommandProcessor(),
            "save" => new SaveTemplateCommandProcessor(),
            "remove" => new RemoveTemplateCommandProcessor(),
            "init" => new InitTemplateCommandProcessor(),
            "list" => new ListTemplatesCommandProcessor(),
            _ => new InvalidCommandProcessor(command)
        };
        
        argsList.RemoveAt(0);
        
        try
        {
            processor.Process(argsList);
        }
        catch (TemplateNotFoundException)
        {
            Console.WriteLine("Template with given name not found");
        }
        catch (ConfigurationInvalidFormatException)
        {
            Console.WriteLine("Configuration in invalid format, try re initiliaze");
        }
        catch (ConfigurationNotFoundException)
        {
            Console.WriteLine("Not found configuration at given path.");
        }
        catch (DirectoryNotFoundException)
        {
            Console.WriteLine("Directory at given path not found.");
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine("Can't get access at given path.");
        }
        catch (IOException)
        {
            Console.WriteLine("IO error occured, probably insufficient permissions.");
        }

        return 0;
    }
}