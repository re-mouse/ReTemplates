namespace ReTemplate;

public class Template
{
    public TemplateDirectory RootDirectory { get; }
    public TemplateConfiguration Configuration { get; }
    
    public Template(TemplateDirectory rootDirectory, TemplateConfiguration configuration)
    {
        RootDirectory = rootDirectory;
        Configuration = configuration;
    }
}