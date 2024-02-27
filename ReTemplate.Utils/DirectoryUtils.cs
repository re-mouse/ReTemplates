namespace ReTemplate.Utils;

public class DirectoryUtils
{
    public delegate void TextFileAction(string path, string text);
    
    public static void ForEachFileInDirectoryRecursively(string path, TextFileAction action)
    {
        if (!Directory.Exists(path))
            throw new DirectoryNotFoundException($"Directory at path {path} not exist");

        foreach (var filePath in Directory.GetFiles(path))
        {
            var text = File.ReadAllText(filePath);
            action.Invoke(filePath, text);
        }

        foreach (var directory in Directory.GetDirectories(path))
        {
            ForEachFileInDirectoryRecursively(directory, action);
        }
    }
}