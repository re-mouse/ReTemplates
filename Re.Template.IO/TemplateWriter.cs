using ReDI;

namespace Re.Template.IO;

public class TemplateWriter
{
    [Inject] private TemplateConfigurationWriter _configurationWriter;

    public void Write(string path, Template template)
    {
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        
        _configurationWriter.Write(path, template.Configuration);

        foreach (var directory in template.Root.Folders)
        {
            WriteFolder(path, directory);
        }
        
        foreach (var file in template.Root.Items)
        {
            SaveItem(path, file);
        }
    }

    private void WriteFolder(string path, TemplateFolder folder)
    {
        var folderPath = Path.Combine(path, folder.Name);

        if (!Directory.Exists(folderPath)) 
        {
            Directory.CreateDirectory(folderPath);
        }
        
        foreach (var file in folder.Items)
        {
            SaveItem(folderPath, file);
        }

        foreach (var subfolder in folder.Folders)
        {
            WriteFolder(folderPath, subfolder);
        }
    }
    
    private void SaveItem(string path, TemplateItem item)
    {
        var filePath = Path.Combine(path, item.Name);
            
        File.WriteAllText(filePath, item.Text);
    }
}