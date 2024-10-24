using Re.Template.IO;
using Re.Template.Repository;
using ReDI;

namespace Re.Template;

public class CoreModule : Module
{
    public override void BindDependencies(TypeManager typeBinder)
    {
        typeBinder.AddSingleton<TemplateTextFormatter>();
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