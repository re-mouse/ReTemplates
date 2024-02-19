namespace ReTemplate.CLI.Consts;

public static class Error
{
    public const string EmptyArguments = 
@"Usage: retemplate [Command] [Options]
 
Commands:
    create          Create files and directories from template
    init            Create configuration file
    save            Save template in registry
    list            List all templates in registry
    remove          Remove template from registry";
    
    public const string InvalidCommandError = 
@"retemplate: {0} is not a command

Commands:
    create          Create files and directories from template
    init            Create configuration file
    save            Save template in registry
    list            List all templates in registry
    remove          Remove template from registry";
    
    public const string TemplateNotFoundError = 
@"template '{0}' not found";
    
    public const string TemplateAtPathNotFoundError = 
@"not found template in path '{0}'";
    
    public const string PathNotExistError = 
@"given path '{0}' is not exist";
    
    public const string TooManyArgumentsError = 
@"retemplate: too many arguments";
}