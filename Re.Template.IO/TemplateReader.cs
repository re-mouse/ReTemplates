using Re.Template.Utils;
using ReDI;

namespace Re.Template.IO;

public class TemplateReader
{
    [Inject] private TemplateConfigurationReader _configurationReader;

    public Template Read(string path)
    {
        if (!Directory.Exists(path))
            throw new DirectoryNotFoundException();
        
        var configuration = _configurationReader.Read(path);
        if (configuration == null)
            throw new ConfigurationNotFoundException();
        
        var directory = new TemplateFolder("Root");
        
        DirectoryUtils.ForEachFileInDirectoryRecursively(path, (templateFilePath, text) =>
        {
            var relativePath = Path.GetRelativePath(path, templateFilePath);
            
            if (configuration?.ExcludeFiles.Contains(relativePath) ?? false)
                return;
            
            CreateTextFileAndAddToFolder(directory, relativePath, text);
        });
        
        return new Template(directory, configuration);
    }

    private void CreateTextFileAndAddToFolder(TemplateFolder rootFolder, string relativePath, string text)
    {
        var separatedPath = relativePath.Split(Path.DirectorySeparatorChar);
        var fileName = separatedPath.Last();

        var currentFolder = FindOrCreateFolder(rootFolder, separatedPath);

        var projectFile = new TemplateItem(fileName, text);
        currentFolder.Items.Add(projectFile);
    }

    private TemplateFolder FindOrCreateFolder(TemplateFolder rootFolder, string[] separatedPath)
    {
        var currentFolder = rootFolder;

        if (separatedPath.Length > 1)
        {
            for (int i = 0; i < separatedPath.Length - 1; i++)
            {
                var subdirectoryName = separatedPath[i];
                var directory = currentFolder.Folders.FirstOrDefault(d => d.Name == subdirectoryName);
                if (directory == null)
                {
                    directory = new TemplateFolder(subdirectoryName);
                    currentFolder.Folders.Add(directory);
                }

                currentFolder = directory;
            }
        }

        return currentFolder;
    }
}