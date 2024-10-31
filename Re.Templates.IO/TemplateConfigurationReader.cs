using ReDI;
using Re.Templates.IO;
using YamlDotNet.Serialization;

namespace Re.Templates.IO;

public class TemplateConfigurationReader
{
    [Inject] private TemplateConfigurationManager _configurationManager;
    
    public TemplateConfiguration? Read(string path)
    {
        var configPath = _configurationManager.GetConfigurationPath(path);
        
        if (File.Exists(configPath))
        {
            try
            {
                using (var fs = File.Open(configPath, FileMode.Open))
                {
                    using (var sw = new StreamReader(fs))
                    {
                        var serializer = new Deserializer();
                        return serializer.Deserialize<TemplateConfiguration>(sw);
                    }
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        return null;
    }
}