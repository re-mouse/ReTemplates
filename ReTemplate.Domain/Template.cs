namespace ReTemplate;

public class Template
{
    public TemplateFolder Root { get; }
    public TemplateConfiguration Configuration { get; }
    
    public Template(TemplateFolder root, TemplateConfiguration configuration)
    {
        Root = root;
        Configuration = configuration;
    }
}