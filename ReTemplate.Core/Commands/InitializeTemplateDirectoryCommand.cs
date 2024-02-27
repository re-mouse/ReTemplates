using ReDI;
using ReTemplate.IO;

namespace ReTemplate.Commands;

public class InitializeTemplateDirectoryCommand
{
    [Inject] private TemplateConfigurationWriter _configurationWriter;

    public void InitializeDirectory(string path, string templateName)
    {
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        var configuration = new TemplateConfiguration(templateName);
        _configurationWriter.Write(path, configuration);
    }
}