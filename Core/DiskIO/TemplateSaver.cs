namespace ReTemplate.IO;

public class TemplateSaver
{
    private readonly TemplateConfigurationSaver _configurationSaver = new TemplateConfigurationSaver();
    
    public void SaveInDirectory(string path, Template template)
    {
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        
        _configurationSaver.SaveInDirectory(path, template.Configuration);

        foreach (var directory in template.RootDirectory.Directories)
        {
            SaveDirectory(path, directory);
        }
        
        foreach (var file in template.RootDirectory.Files)
        {
            SaveFile(path, file);
        }
    }

    private void SaveDirectory(string path, TemplateDirectory directory)
    {
        var folderPath = Path.Combine(path, directory.Name);

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
        
        foreach (var file in directory.Files)
        {
            SaveFile(path, file);
        }

        foreach (var subfolder in directory.Directories)
        {
            SaveDirectory(folderPath, subfolder);
        }
    }
    
    private void SaveFile(string path, TemplateFile file)
    {
        var filePath = Path.Combine(path, file.Name);
            
        File.WriteAllText(filePath, file.Text);
    }
}