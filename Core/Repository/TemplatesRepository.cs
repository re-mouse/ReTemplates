using ReTemplate.IO;

namespace ReTemplate.Repository;

public class TemplatesRepository
{
    private readonly TemplateReader _reader = new TemplateReader();
    private readonly TemplateSaver _saver = new TemplateSaver();
    private readonly TemplateConfigurationReader _configurationReader = new TemplateConfigurationReader();
    private readonly TemplateConfigurationSaver _configurationSaver = new TemplateConfigurationSaver();
    
    public List<string> GetAllTemplateNames()
    {
        var templateNames = new List<string>();
        var repositoryPath = RepositoryPathConst.RepositoryPath;
        if (!Directory.Exists(repositoryPath))
            return templateNames;

        foreach (var directoryPath in Directory.GetDirectories(repositoryPath))
        {
            var configuration = _configurationReader.GetFromDirectory(directoryPath);
            if (configuration == null)
                continue;
            
            templateNames.Add(configuration.Name);
        }

        return templateNames;
    }

    public Template GetTemplateByName(string templateName)
    {
        var templateDirectoryPath = Path.Combine(RepositoryPathConst.RepositoryPath, templateName);
        
        if (!Directory.Exists(templateDirectoryPath))
            throw new TemplateNotFoundException();

        var template = _reader.Read(templateDirectoryPath);
        return template;
    }
    
    public void Init(string directoryPath, string templateName)
    {
        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);

        var configuration = new TemplateConfiguration(templateName);
        _configurationSaver.SaveInDirectory(directoryPath, configuration);
    }
    
    public void Remove(string templateName)
    {
        var templatePath = Path.Combine(RepositoryPathConst.RepositoryPath, templateName);
        
        if (!Directory.Exists(templatePath))
            throw new TemplateNotFoundException();
        
        Directory.Delete(templatePath);
    }

    public void Save(string templatePath)
    {
        var template = _reader.Read(templatePath);
        
        var savingPath = Path.Combine(RepositoryPathConst.RepositoryPath, template.Configuration.Name);
        
        _saver.SaveInDirectory(savingPath, template);
    }
}