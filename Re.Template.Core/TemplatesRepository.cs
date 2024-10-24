
using Re.Template.IO;
using ReDI;

namespace Re.Template.Repository;

public class TemplatesRepository
{
    private static readonly string ApplicationDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    private static readonly string RepositoryPath = $"{ApplicationDataFolder}/reproject/templates";
    
    [Inject] private TemplateReader _reader;
    [Inject] private TemplateWriter _saver;
    [Inject] private TemplateConfigurationReader _configurationReader;
    
    public List<string> GetAllTemplateNames()
    {
        var templateNames = new List<string>();
        var repositoryPath = RepositoryPath;
        
        if (!Directory.Exists(repositoryPath))
            return templateNames;

        foreach (var path in Directory.GetDirectories(repositoryPath))
        {
            var configuration = _configurationReader.Read(path);
            if (configuration == null)
                continue;
            
            templateNames.Add(configuration.Name);
        }

        return templateNames;
    }

    public Template GetTemplateByName(string templateName)
    {
        var templatepath = GetTemplatePath(templateName);
        
        if (!Directory.Exists(templatepath))
            throw new TemplateNotFoundException();

        var template = _reader.Read(templatepath);
        return template;
    }
    
    public void Remove(string templateName)
    {
        var templatePath = GetTemplatePath(templateName);
        
        if (!Directory.Exists(templatePath))
            throw new TemplateNotFoundException();
        
        Directory.Delete(templatePath, true);
    }

    public void Save(Template template)
    {
        var savingPath = GetTemplatePath(template.Configuration.Name);

        if (Directory.Exists(savingPath))
        {
            Directory.Delete(savingPath, true);
        }
        _saver.Write(savingPath, template);
    }
    
    private static string GetTemplatePath(string templateName) { return Path.Combine(RepositoryPath, templateName); }
}