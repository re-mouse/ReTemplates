using System.Text.Json;

namespace ReTemplate.IO;

public class TemplateConfigurationSaver
{
    public void SaveInDirectory(string path, TemplateConfiguration configuration)
    {
        var configPath = Path.Combine(path, TemplatePathConst.ConfigurationFile);
        
        using (var fs = File.Open(configPath, FileMode.OpenOrCreate))
        {
            JsonSerializer.Serialize(fs, configuration);
        }
    }
}