namespace Re.Template.Utils;

public class PathUtils
{
    public static string ConvertInputToFullPath(string input)
    {
        if (input.StartsWith("~"))
        {
            input = input.Substring(1);
            if (input.StartsWith("/"))
                input = input.Substring(1);
            return Path.Combine(GetHomeFolder(), input);
        }
        else return Path.GetFullPath(input);
    }

    public static string GetHomeFolder()
    {
        return ((Environment.OSVersion.Platform == PlatformID.Unix || 
                           Environment.OSVersion.Platform == PlatformID.MacOSX)
            ? Environment.GetEnvironmentVariable("HOME")
            : Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%")) ?? throw new ApplicationException();
    }
}