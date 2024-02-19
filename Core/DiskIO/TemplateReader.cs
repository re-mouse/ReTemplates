namespace ReTemplate.IO;

public class TemplateReader
{
    private readonly TemplateConfigurationReader _configurationReader = new TemplateConfigurationReader();
    public Template Read(string path)
    {
        if (!Directory.Exists(path))
            throw new DirectoryNotFoundException();
        
        var configuration = _configurationReader.GetFromDirectory(path);
        if (configuration == null)
            throw new ConfigurationNotFoundException();
        
        var directory = new TemplateDirectory("Root");
        
        DirectoryUtils.ForEachFileInDirectoryRecursively(path, (templateFilePath, text) =>
        {
            var relativePath = Path.GetRelativePath(path, templateFilePath);
            
            if (configuration?.ExcludeFiles.Contains(relativePath) ?? false)
                return;
            
            CreateTextFileAndAddToFolder(directory, relativePath, text);
        });
        
        return new Template(directory, configuration);
    }

    private void CreateTextFileAndAddToFolder(TemplateDirectory rootDirectory, string relativePath, string text)
    {
        var separatedPath = relativePath.Split(Path.DirectorySeparatorChar);
        var fileName = separatedPath.Last();

        var currentFolder = FindOrCreateFolder(rootDirectory, separatedPath);

        var projectFile = new TemplateFile(fileName, text);
        currentFolder.Files.Add(projectFile);
    }

    private TemplateDirectory FindOrCreateFolder(TemplateDirectory rootDirectory, string[] separatedPath)
    {
        var currentFolder = rootDirectory;

        if (separatedPath.Length > 1)
        {
            for (int i = 0; i < separatedPath.Length - 1; i++)
            {
                var subdirectoryName = separatedPath[i];
                var directory = currentFolder.Directories.FirstOrDefault(d => d.Name == subdirectoryName);
                if (directory == null)
                {
                    directory = new TemplateDirectory(subdirectoryName);
                    currentFolder.Directories.Add(directory);
                }

                currentFolder = directory;
            }
        }

        return currentFolder;
    }
}