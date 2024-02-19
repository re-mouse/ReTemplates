namespace ReTemplate.Repository;

public static class RepositoryPathConst
{
    private static readonly string ApplicationDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

    public static readonly string RepositoryPath = $"{ApplicationDataFolder}/reproject/templates";
}