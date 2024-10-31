using System.CommandLine;
using System.CommandLine.Parsing;
using Re.Templates;
using Re.Templates.Commands;
using Re.Templates.IO;
using Re.Templates.Utils;
using ReDI;
using Re.Templates.Repository;

namespace Re.Templates.CLI;

public class SaveCliCommand : ICliCommand
{
    [Inject] private TemplateConfigurationReader _configurationReader;
    [Inject] private SaveTemplateFromDiskInRepositoryCommand _command;

    public Command AsCLICommand()
    {
        var pathArgument = new Argument<string>("TemplateDirectory", ParsePath, true, "Path to directory, where template and configuration is stored");
        var command = new Command("save", "Save template in repository");
        command.AddArgument(pathArgument);
        command.SetHandler(RunCommand, pathArgument);
        return command;
    }

    private void RunCommand(string path)
    {
        try
        {
            _command.SaveFromDisk(path);
            var configuration = _configurationReader.Read(path);
            Console.WriteLine($"Template at path {path} saved with name {configuration.Name}");
            return;
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
        Program.StatusCode = 1;
    }
    
    private string ParsePath(ArgumentResult result)
    {
        if (result.Tokens.Count == 0)
        {
            return PathUtils.ConvertInputToFullPath(".");

        }
        string? path = result.Tokens.Single().Value;
        path = PathUtils.ConvertInputToFullPath(path);
        
        if (!Directory.Exists(path))
        {
            result.ErrorMessage = $"Directory {path} not exist";
            return "";
        }
        else
        {
            return PathUtils.ConvertInputToFullPath(path);
        }
    }
}