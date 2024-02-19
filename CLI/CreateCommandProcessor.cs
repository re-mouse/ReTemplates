using ReTemplate.CLI.Consts;
using ReTemplate.IO;
using ReTemplate.Repository;

namespace ReTemplate.CLI;

public class CreateCommandProcessor : ICommandProcessor
{
    private readonly TemplateValidator _templateValidator = new TemplateValidator();
    private readonly TemplateMetadataFactory _templateMetadataFactory = new TemplateMetadataFactory();
    private readonly TemplatesRepository _templatesRepository = new TemplatesRepository();
    private readonly ProjectFactory _projectFactory = new ProjectFactory();
    private readonly ProjectSaver _projectSaver = new ProjectSaver();

    public void Process(List<string> args)
    {
        if (args.Count == 0)
        {
            Console.WriteLine(TemplatesCommandError.EmptyCreateCommand);
            return;
        }

        var commandArgs = CreateCommandArgs.Create(args);
        var template = _templatesRepository.GetTemplateByName(commandArgs.TemplateName);
            
        try
        {
            _templateValidator.Validate(template.RootDirectory);
        }
        catch (FormatException formatException)
        {
            Console.WriteLine($"Error while validated template: {formatException.Message}");
        }
        
        var metadata = _templateMetadataFactory.CreateFromDirectory(template.RootDirectory);
        var templateFormatArgsBuilder = new TemplateFormatArgsFactory(GetCondition, GetPlaceholder, GetArrayCount);
        var formatArgs = templateFormatArgsBuilder.Create(metadata);
        var projectDirectory = _projectFactory.CreateProjectDirectoryFromTemplate(template.RootDirectory, formatArgs);
        var project = new Project(projectDirectory, commandArgs.SavingPath);
        
        _projectSaver.ValidateCanSave(project, true);
        _projectSaver.Save(project);
    }

    private int GetArrayCount(string name, ArrayNode? arrayNode)
    {
        return 0;
    }

    private string GetPlaceholder(string name, ArrayMember? array)
    {
        return ";";
    }

    private bool GetCondition(string name, ArrayMember? array)
    {
        return false;
    }
    
    private class CreateCommandArgs
    {
        public bool Force { get; private set; }
        
        public bool TemplateNameAsPath { get; private set; }

        public string TemplateName { get; private set; }

        public string SavingPath { get; private set; }

        public static CreateCommandArgs Create(List<string> args)
        {
            var commandArgs = new CreateCommandArgs();

            while (args.Count > 0)
            {
                ConsumeArg(args, commandArgs);
            }

            return commandArgs;
        }

        private static void ConsumeArg(List<string> args, CreateCommandArgs commandArgs)
        {
            var arg = args[0];
            args.RemoveAt(0);
            switch (arg)
            {
                case "-f" or "--force" when !commandArgs.Force:
                    commandArgs.Force = true;
                    break;
                case "-p" or "--path" when !commandArgs.TemplateNameAsPath:
                    commandArgs.TemplateNameAsPath = true;
                    break;
                default:
                    if (commandArgs.TemplateName == null)
                        commandArgs.TemplateName = arg;
                    if (commandArgs.SavingPath == null)
                        commandArgs.SavingPath = arg;
                    throw new ArgumentException();
            }
        }
    }
}