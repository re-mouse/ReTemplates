using System.Text.RegularExpressions;
using ReTemplate.Utils;

namespace ReTemplate;

public static class TemplateFormatterHelper
{
    public static int IndexOfTagEnd(Match tagMatch, string text)
    {
        var tag = tagMatch.Groups[0].Value;
        var tagEndIndex = tagMatch.Index + tagMatch.Length;
        var tagStartIndex = tagMatch.Index;
        
        if (TemplateRegex.IsPlaceholderTag(tag))
            return tagEndIndex;
        
        var startNewLineIndex = text.LastIndexOf('\n', tagStartIndex);
        var endNewLineIndex = text.IndexOf('\n', tagStartIndex);
        var end = endNewLineIndex == -1 ? text.Length : endNewLineIndex;
        var start = startNewLineIndex == -1 ? 0 : startNewLineIndex + 1;
        var trimmedFirstLine = text.Substring(start, end - start).Trim();
        
        if (trimmedFirstLine == tagMatch.Value)
        {
            return end + 1 <= text.Length ? end + 1 : text.Length;
        }
        else
        {
            return tagEndIndex;
        }
    }
    
    public static int IndexOfTagStart(Match tagMatch, string text)
    {
        var tag = tagMatch.Groups[0].Value;
        var tagEndIndex = tagMatch.Index + tagMatch.Length;
        var tagStartIndex = tagMatch.Index;
        
        if (TemplateRegex.IsPlaceholderTag(tag))
            return tagStartIndex;
        
        var startNewLineIndex = text.LastIndexOf('\n', tagStartIndex);
        var endNewLineIndex = text.IndexOf('\n', tagStartIndex);
        var end = endNewLineIndex == -1 ? text.Length : endNewLineIndex;
        var start = startNewLineIndex == -1 ? 0 : startNewLineIndex;
        var trimmedFirstLine = text.Substring(start, end - start).Trim();
        
        if (trimmedFirstLine == tag)
        {
            return start + 1;
        }
        else
        {
            return tagStartIndex;
        }
    }
    
    public static int IndexOfConditionClosingTag(MatchCollection tagsMatch, string conditionName, int startIndex)
    {
        return tagsMatch.IndexOf(match =>
        {
            string tag = match.Groups[0].Value;
            string tagName = match.Groups[1].Value;
            return TemplateRegex.IsConditionTag(tag) && TemplateRegex.IsClosingTagName(tagName) &&
                   tagName.Substring(1) == conditionName;
        }, startIndex);
    }

    public static int IndexOfArrayClosingTag(MatchCollection tagsMatch, string arrayName, int startIndex)
    {
        return tagsMatch.IndexOf(match =>
        {
            string tag = match.Groups[0].Value;
            string tagName = match.Groups[1].Value;
            return TemplateRegex.IsArrayTag(tag) && TemplateRegex.IsClosingTagName(tagName) &&
                   tagName.Substring(1) == arrayName;
        }, startIndex);
    }
    
    public static ArrayNode GetArrayConfiguration(List<ArrayNode> arrays, string tagName)
    {
        var nodeName = tagName.Split('.').Last();
        return arrays.First(a => a.Name == nodeName);
    }

    public static Dictionary<string, string> CombinePlaceholdersFromMemberArray(Dictionary<string, string> placeholders, ArrayMember member)
    {
        var memberPlaceholders = new Dictionary<string, string>(placeholders);
        var path = new List<string>();

        var node = member.Node;
        while (node != null)
        {
            path.Add(node.Name);
            node = node.Parent?.Node;
        }

        path.Reverse();

        foreach (var placeholder in member.Placeholder)
        {
            var fullName = $"{string.Join('.', path)}.{placeholder.Key}";
            memberPlaceholders[fullName] = placeholder.Value;
        }

        return memberPlaceholders;
    }

    public static List<string> CombineConditionsFromMemberArray(List<string> conditions, ArrayMember member)
    {
        var memberConditions = new List<string>(conditions);
        var path = new List<string>();

        var tempArrayConfiguration = member.Node;
        while (tempArrayConfiguration != null)
        {
            path.Add(tempArrayConfiguration.Name);
            tempArrayConfiguration = tempArrayConfiguration.Parent?.Node;
        }

        path.Reverse();

        foreach (var condition in member.Definitions)
        {
            var fullName = $"{string.Join('.', path)}.{condition}";
            memberConditions.Add(fullName);
        }

        return memberConditions;
    }
}