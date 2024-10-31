using System.CommandLine;
using System.CommandLine.Parsing;
using Re.Templates.Commands;
using Re.Templates.IO;
using Re.Templates.Utils;
using ReDI;

namespace Re.Templates.CLI;

public class InitCliCommand : ICliCommand
{
    [Inject] private InitializeTemplateDirectoryCommand _command;
    [Inject] private DirectoryManager _directoryManager;

    public Command AsCLICommand()
    {
        var command = new Command("init", "Initialize template configuratioin at given directory");
        var templateNameArgument = new Argument<string>("TemplateName", ParseTemplateName, true, "That name will be used in configuration");
        var pathArgument = new Argument<string>("TemplateDirectory", ParsePath, true, "Path to directory, where need to initialize configuration");
        command.AddArgument(pathArgument);
        command.AddArgument(templateNameArgument);
        command.SetHandler(RunCommand, pathArgument, templateNameArgument);
        return command;
    }

    private void RunCommand(string path, string templateName)
    {
        try
        {
            _command.InitializeDirectory(path, templateName);
            Console.WriteLine($"Created config for template with name {templateName} at path {path}");
            return;
        }
        catch (IOException)
        {
            Console.WriteLine("IO error occured, probably insufficient permissions.");
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine("Unathorized access error occured");
        }
        Program.StatusCode = 1;
    }

    private string ParseTemplateName(ArgumentResult result)
    {
        if (result.Tokens.Count == 0)
        {
            return "Default";
        }
        
        return result.Tokens.Single().Value;
    }

    private string ParsePath(ArgumentResult result)
    {
        if (result.Tokens.Count == 0)
        {
            return PathUtils.ConvertInputToFullPath(".");

        }
        string? path = result.Tokens.Single().Value;
        path = PathUtils.ConvertInputToFullPath(path);
        
        try
        {
            _directoryManager.ValidateCanCreateFiles(path);
        }
        catch (Exception)
        {
            result.ErrorMessage = $"Cannot create files at {path}";
            return "";
        }
        
        if (!Directory.Exists(path))
        {
            result.ErrorMessage = $"Directory {path} not exist";
            return null;
        }
        else
        {
            return PathUtils.ConvertInputToFullPath(path);
        }
    }
}