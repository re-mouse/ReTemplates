using System.Text.RegularExpressions;

namespace ReTemplate;

public class TemplateRegex
{
    private const string ArrayRegexPattern = "!!!([^!]+)!!!";
    private const string ConditionRegexPattern = "\\[\\[\\[([^!]+)\\]\\]\\]";
    private const string PlaceholderRegexPattern = "\\{\\{\\{([^!]+)\\}\\}\\}";
    private const string AnyTagRegexPattern = "(?:!!!|\\[\\[\\[|\\{\\{\\{)([^!{}\\[\\]]+?)(?:!!!|\\]\\]\\]|\\}\\}\\})";
    
    public static readonly Regex ArrayTagRegex = new Regex(ArrayRegexPattern);
    public static readonly Regex ConditionTagRegex = new Regex(ConditionRegexPattern);
    public static readonly Regex PlaceholderTagRegex = new Regex(PlaceholderRegexPattern);
    public static readonly Regex AnyTagRegex = new Regex(AnyTagRegexPattern);
    
    public static bool IsPlaceholderTag(string tag)
    {
        return PlaceholderTagRegex.Match(tag).Success;
    }

    public static bool IsArrayTag(string tag)
    {
        return ArrayTagRegex.Match(tag).Success;
    }
    
    public static bool IsConditionTag(string tag)
    {
        return ConditionTagRegex.Match(tag).Success;
    }
    
    public static bool IsClosingTagName(string tagName)
    {
        return tagName.StartsWith('/');
    }
}