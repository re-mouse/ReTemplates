using System.CommandLine;
using System.CommandLine.Parsing;
using ReDI;
using ReTemplate.Commands;
using ReTemplate.IO;
using ReTemplate.Utils;

namespace ReTemplate.CLI;

public class CreateCliCommand : ICliCommand
{
    [Inject] private CreateArtifactFromRepositoryTemplateCommand _repositoryCommand;
    [Inject] private CreateArtifactFromDiskTemplateCommand _diskCommand;
    [Inject] private DirectoryManager _directoryManager;

    public Command AsCLICommand()
    {
        var command = new Command("create", "Create formatted files from template recursively");
        var overrideOption = new Option<bool>(new[] { "--force", "-f" }, "Override existing files on interception");
        var templatePathOption = new Option<bool>(new[] { "--path", "-p" }, "Use template name as template path");
        var templateNameArgument = new Argument<string>("TemplateName", "Template name of which you want to create files, search for template in repository");
        var savingPathArgument = new Argument<string>("SavingPath", ParsePath, true, "Path to directory where files will be saved, create all mising subdirectories");
        command.AddOption(overrideOption);
        command.AddOption(templatePathOption);
        command.AddArgument(templateNameArgument);
        command.AddArgument(savingPathArgument);
        command.SetHandler(RunCommand, templateNameArgument, savingPathArgument, templatePathOption, overrideOption);
        return command;
    }

    private void RunCommand(string templateName, string savingPath, bool templatePathFromName, bool canOverride)
    {
        try
        {
            if (templatePathFromName)
            {
                _diskCommand.Create(templateName, savingPath, canOverride);
            }
            else
            {
                _repositoryCommand.Create(templateName, savingPath, canOverride);
            }
        }
        catch (TemplateNotFoundException)
        {
            Console.WriteLine($"Template {templateName} not found.");
            Console.WriteLine($"Ensure you saved template in repository, or use --path option");
            Console.WriteLine($"See --help for more information");
            return;
        }

        Console.WriteLine($"Created files from template {templateName} at {savingPath}");
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
            try
            {
                _directoryManager.ValidateCanCreateDirectory(path);
            }
            catch (Exception)
            {
                result.ErrorMessage = $"Cannot create directory or subdirectory at {path}";
            }
        }
        else
        {
            try
            {
                _directoryManager.ValidateCanCreateFiles(path);
            }
            catch (Exception)
            {
                result.ErrorMessage = $"Cannot create files at {path}";
                return "";
            }
        }
        
        return path;
    }
}