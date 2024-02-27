using ReDI;
using ReTemplate.Commands;

namespace ReTemplate;

public class CommandsModule : Module
{
    public override void BindDependencies(TypeManager typeBinder)
    {
        typeBinder.AddSingleton<CreateArtifactFromDiskTemplateCommand>();
        typeBinder.AddSingleton<CreateArtifactFromRepositoryTemplateCommand>();
        typeBinder.AddSingleton<InitializeTemplateDirectoryCommand>();
        typeBinder.AddSingleton<RemoveTemplateFromRepositoryCommand>();
        typeBinder.AddSingleton<ValidateTemplateDirectoryCommand>();
        typeBinder.AddSingleton<SaveTemplateFromDiskInRepositoryCommand>();
    }

    public override void BindModuleDependencies(ModuleManager moduleBinder)
    {
        
    }
}