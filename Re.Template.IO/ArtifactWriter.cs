using ReDI;
using Re.Template.IO;

namespace Re.Template.IO;

public class ArtifactWriter
{
    [Inject] private DirectoryManager _directoryManager;
    
    public void ValidateCanSave(Artifact artifact, string path, bool canOverride)
    {
        _directoryManager.ValidateCanCreateDirectory(path);
        
        foreach (var subFolder in artifact.Root.Folders)
        {
            ValidateCanWriteFilesFromDirectory(path, subFolder, canOverride);
        }
    }

    private void ValidateCanWriteFilesFromDirectory(string path, ArtifactFolder folder, bool canOverride)
    {
        var directory = Path.Combine(path, folder.Name);
        if (!Directory.Exists(path))
        {
            return;
        }

        _directoryManager.ValidateCanCreateFiles(directory);
        
        foreach (var file in folder.Items)
        {
            var filePath = Path.Combine(directory, file.Name);
            
            if (!canOverride && File.Exists(filePath))
            {
                throw new CannotOverrideExistingFileException(filePath);
            }
        }
        
        foreach (var subFolder in folder.Folders)
        {
            ValidateCanWriteFilesFromDirectory(directory, subFolder, canOverride);
        }
    }

    public void Write(Artifact artifact, string path, bool canOverride)
    {
        ValidateCanSave(artifact, path, canOverride);
        
        _directoryManager.CreateIfNotExist(path);

        foreach (var item in artifact.Root.Items)
        {
            var filePath = Path.Combine(path, item.Name);
            
            File.WriteAllText(filePath, item.Text);
        }

        foreach (var subfolder in artifact.Root.Folders)
        {
            SaveFolder(path, subfolder);
        }
    }

    private void SaveFolder(string path, ArtifactFolder folder)
    {
        var directory = Path.Combine(path, folder.Name);

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        foreach (var item in folder.Items)
        {
            var filePath = Path.Combine(directory, item.Name);
            
            File.WriteAllText(filePath, item.Text);
        }

        foreach (var subfolder in folder.Folders)
        {
            SaveFolder(directory, subfolder);
        }
    }
}