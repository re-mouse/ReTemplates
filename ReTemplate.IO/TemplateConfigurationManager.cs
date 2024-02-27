namespace ReTemplate.IO;

internal class TemplateConfigurationManager
{
    private const string ConfigurationFile = "template.yaml";

    public string? GetConfigurationPath(string directory)
    {
        return Path.Combine(directory, ConfigurationFile);
    }
}