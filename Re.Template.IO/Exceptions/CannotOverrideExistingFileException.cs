namespace Re.Template.IO;

public class CannotOverrideExistingFileException : Exception
{
    private readonly string _filePath;

    public CannotOverrideExistingFileException(string filePath)
    {
        _filePath = filePath;
    }

    public override string Message { get => $"File exist and cannot be overrided. Delete or enable override. File {_filePath}"; }
}