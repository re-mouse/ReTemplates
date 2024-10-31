namespace Re.Templates.IO;

public class DirectoryManager
{
    public string[] GetSubdirectories(string path)
    {
        return Directory.GetDirectories(path);
    }
    
    public void CreateIfNotExist(string path)
    {
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
    }
    
    public void ValidateCanCreateDirectory(string path)
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
    
    public void ValidateCanCreateFiles(string path)
    {
        using (var fs = File.Create(Path.Combine(path, "BipBopAramZamZamTestFileCreation___111@3344%59599120gm,vnb"), 1, FileOptions.DeleteOnClose))
        {
        }
    }
}