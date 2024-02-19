using ReTemplate;

public class ProjectFactory
{
    TemplateFormatter _formatter = new TemplateFormatter();

    public ProjectDirectory CreateProjectDirectoryFromTemplate(TemplateDirectory templateDirectory, TemplateFormatArgs args)
    {
        var directory = new ProjectDirectory(templateDirectory.Name);
        foreach (var file in templateDirectory.Files)
        {
            var formattedText = _formatter.Format(file.Text, args);
            directory.Files.Add(new ProjectFile(file.Name, formattedText));
        }

        foreach (var templateSubdirectory in templateDirectory.Directories)
        {
            directory.Directories.Add(CreateProjectDirectoryFromTemplate(templateSubdirectory, args));
        }

        return directory;
    }
}