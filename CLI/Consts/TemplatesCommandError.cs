namespace ReTemplate.CLI.Consts;

public static class TemplatesCommandError
{
    public const string NameStartsWithSlash = 
@"'retemplate': template name cannot start with '/'";
    
    public const string EmptyRemoveCommand = 
@"Usage: retemplate remove [template-name]";
    
    public const string EmptySaveCommand = 
@"Usage: retemplate save
       retemplate save [template-directory]

template-directory:
    The path to directory, where template located";
    
    public const string EmptyInitCommand =
@"Usage: reproject init [template-name]
       reproject init [template-name] [template-directory]

template-directory:
    The path to directory, where template files located";
    
    public const string EmptyCreateCommand = 
@"Usage: reproject create [Options] [template-name]
       reproject create [Options] [template-name] [target-directory]

Options:
  -p|--path         Uses template-name as path to template directory
  -f|--force        Override files with same name in target directory

target-directory:
    The path to directory, where template files will be created";
    
    public const string MissingTemplateNameArgument = 
@"'reproject create' missing template name argument";
    
    public const string CannotCreateInDirectory = 
@"cannot create project in '{0}'";
}