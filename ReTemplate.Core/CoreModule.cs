using ReDI;
using ReTemplate.IO;
using ReTemplate.Repository;

namespace ReTemplate;

public class CoreModule : Module
{
    public override void BindDependencies(TypeManager typeBinder)
    {
        typeBinder.AddSingleton<TemplateFormatter>();
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