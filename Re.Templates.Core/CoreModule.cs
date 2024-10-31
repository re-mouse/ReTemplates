using Re.Templates.IO;
using Re.Templates.Repository;
using ReDI;

namespace Re.Templates;

public class CoreModule : Module
{
    public override void BindDependencies(TypeManager typeBinder)
    {
        typeBinder.AddSingleton<TemplateTextFormatter>();
        typeBinder.AddSingleton<TemplateFileFormatter>();
        typeBinder.AddSingleton<TemplateDirectoryFormatter>();
        typeBinder.AddSingleton<TemplateMetadataFactory>();
        typeBinder.AddSingleton<TemplateValidator>();
        typeBinder.AddSingleton<ArtifactFactory>();
        typeBinder.AddSingleton<TemplateFormatArgsFactory>();
        typeBinder.AddSingleton<TemplatesRepository>();
    }

    public override void BindModuleDependencies(ModuleManager moduleBinder)
    {
        moduleBinder.RegisterModule<IOModule>();
    }
}