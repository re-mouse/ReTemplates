using System.CommandLine;
using System.CommandLine.Parsing;
using ReDI;
using ReTemplate.Commands;
using ReTemplate.Utils;

namespace ReTemplate.CLI;

public class ValidateCliCommand : ICliCommand
{
    [Inject] private ValidateTemplateDirectoryCommand _command;

    public Command AsCLICommand()
    {
        var pathArgument = new Argument<string>("TemplateDirectory", ParsePath, true, "Path to directory, that have to be validated. Require configuration");
        var command = new Command("validate", "Validate template at given path");
        command.AddArgument(pathArgument);
        command.SetHandler(RunCommand, pathArgument);
        return command;
    }

    private void RunCommand(string path) 
    {
        try
        {
            _command.Validate(path);
        }
        catch (FormatException e)
        {
            Console.WriteLine($"Error while validated template: {e.Message}");
        }
        catch (IOException)
        {
            Console.WriteLine("IO error occured, probably insufficient permissions.");
        }
        catch (ConfigurationInvalidFormatException)
        {
            Console.WriteLine("Configuration in invalid format, try initiliaze again");
        }
        catch (ConfigurationNotFoundException)
        {
            Console.WriteLine($"Not found configuration at {path}");
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine("Unathorized access error occured");
        }
    }

    private string ParsePath(ArgumentResult result)
    {
        if (result.Tokens.Count == 0)
        {
            return PathUtils.ConvertInputToFullPath(".");
        }
        
        string path = result.Tokens.Single().Value;
        path = PathUtils.ConvertInputToFullPath(path);
        
        if (!Directory.Exists(path))
        {
            result.ErrorMessage = "Directory not exist";
            return "";
        }
        else
        {
            return path;
        }
    }
}