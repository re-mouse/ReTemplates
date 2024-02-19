namespace ReTemplate.IO;

public class ProjectSaver
{
    public void ValidateCanSave(Project project, bool canOverride)
    {
        if (!Directory.Exists(project.Path))
        {
            CreateAndDeleteMissingDirectory(project.Path);
        }
        else
        {
            CreateAndDeleteFileInDirectory(project.Path);
            
            foreach (var subFolder in project.RootDirectory.Directories)
            {
                ValidateCanWriteFilesFromDirectory(project.Path, subFolder, canOverride);
            }
        }
    }

    private void CreateAndDeleteMissingDirectory(string path)
    {
        string[] pathParts = path.Split(Path.DirectorySeparatorChar);

        string partialPath = pathParts[0];
        for (int i = 1; i < pathParts.Length; i++)
        {
            partialPath = Path.Combine(partialPath, pathParts[i]);

            if (!Directory.Exists(partialPath))
            {
                Directory.CreateDirectory(partialPath);
                Directory.Delete(partialPath);

                break;
            }
        }
    }

    private void CreateAndDeleteFileInDirectory(string path)
    {
        using (var fs = File.Create(Path.Combine(path, "BipBopAramZamZamTestFileCreation___111@3344%59599120gm,vnb"), 1, FileOptions.DeleteOnClose))
        {
        }
    }

    private void ValidateCanWriteFilesFromDirectory(string path, ProjectDirectory directory, bool canOverride)
    {
        var folderPath = Path.Combine(path, directory.Name);
        if (!Directory.Exists(path))
        {
            return;
        }

        CreateAndDeleteFileInDirectory(path);
        
        foreach (var file in directory.Files)
        {
            var filePath = Path.Combine(folderPath, file.Name);
            if (!canOverride && File.Exists(filePath))
            {
                throw new FileAlreadyExistException($"File exist and cannot be overrided. Delete or enable override. File {filePath}");
            }
            else if (File.Exists(filePath))
            {
                using (FileStream fs = File.Open(filePath, FileMode.Open, FileAccess.Write))
                {
                }
            }
        }
        
        foreach (var subFolder in directory.Directories)
        {
            ValidateCanWriteFilesFromDirectory(path, subFolder, canOverride);
        }
    }

    public void Save(Project project)
    {
        if (!Directory.Exists(project.Path))
        {
            Directory.CreateDirectory(project.Path);
        }

        foreach (var file in project.RootDirectory.Files)
        {
            var filePath = Path.Combine(project.Path, file.Name);
            
            File.WriteAllText(filePath, file.Text);
        }

        foreach (var subfolder in project.RootDirectory.Directories)
        {
            SaveFolder(project.Path, subfolder);
        }
    }

    private void SaveFolder(string path, ProjectDirectory directory)
    {
        var folderPath = Path.Combine(path, directory.Name);

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
        foreach (var file in directory.Files)
        {
            var filePath = Path.Combine(folderPath, file.Name);
            
            File.WriteAllText(filePath, file.Text);
        }

        foreach (var subfolder in directory.Directories)
        {
            SaveFolder(folderPath, subfolder);
        }
    }
}

public class FileAlreadyExistException : Exception
{
    public FileAlreadyExistException(string message) : base(message)
    {
    }
}