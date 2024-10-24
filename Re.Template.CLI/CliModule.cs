using ReDI;

namespace Re.Template.CLI;

public class CliModule : Module
{
    public override void BindDependencies(TypeManager typeBinder)
    {
        typeBinder.AddSingleton<ICliCommand, CreateCliCommand>();
        typeBinder.AddSingleton<ICliCommand, RemoveCliCommand>();
        typeBinder.AddSingleton<ICliCommand, ListCliCommand>();
        typeBinder.AddSingleton<ICliCommand, InitCliCommand>();
        typeBinder.AddSingleton<ICliCommand, SaveCliCommand>();
        typeBinder.AddSingleton<ICliCommand, ValidateCliCommand>();
        typeBinder.AddSingleton<RootCliCommandFactory>();
        typeBinder.AddSingleton<ITemplateArgsGetter, CliTemplateArgsGetter>();
    }

    public override void BindModuleDependencies(ModuleManager moduleBinder)
    {
        moduleBinder.RegisterModule<CoreModule>();
        moduleBinder.RegisterModule<CommandsModule>();
    }
}