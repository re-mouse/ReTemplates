using System.Text.Json;

namespace ReTemplate.IO;

public class TemplateConfigurationReader
{
    public TemplateConfiguration? GetFromDirectory(string path)
    {
        var configPath = Path.Combine(path, TemplatePathConst.ConfigurationFile);
        if (File.Exists(configPath))
        {
            try
            {
                using (var fs = File.Open(configPath, FileMode.Open))
                {
                    return JsonSerializer.Deserialize<TemplateConfiguration>(fs);
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