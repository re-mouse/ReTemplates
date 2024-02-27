using ReDI;
using ReTemplate.IO;
using YamlDotNet.Serialization;

namespace ReTemplate.IO;

public class TemplateConfigurationWriter
{
    [Inject] private TemplateConfigurationManager _manager;
    
    public void Write(string path, TemplateConfiguration configuration)
    {
        var configPath = _manager.GetConfigurationPath(path);
        
        using (var fs = File.Open(configPath, FileMode.OpenOrCreate))
        {
            using (var sw = new StreamWriter(fs))
            {
                var serializer = new Serializer();
                serializer.Serialize(sw, configuration);
            }
        }
    }
}