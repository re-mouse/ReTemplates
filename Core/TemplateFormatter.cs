using System.Text;
using System.Text.RegularExpressions;

namespace ReTemplate;

public class TemplateFormatter
{
    public string Format(string text, TemplateFormatArgs args)
    {
        var stringBuilder = new StringBuilder();
        Format(text, stringBuilder, args.Definitions, args.Placeholder, args.Arrays);
        return stringBuilder.ToString();
    }

    private void Format(string text, StringBuilder builder, List<string> conditions,
        Dictionary<string, string> placeholders, List<ArrayNode> arrays)
    {
        var tagsMatch = TemplateRegex.AnyTagRegex.Matches(text);
        if (tagsMatch.Count == 0)
        {
            builder.Append(text);
            return;
        }

        AppendTextBeforeFirstTag(text, builder, tagsMatch);

        for (int i = 0; i < tagsMatch.Count; i++)
        {
            var match = tagsMatch[i];
            string tag = match.Groups[0].Value;
            string tagName = match.Groups[1].Value;

            if (TemplateRegex.IsPlaceholderTag(tag))
            {
                AppendPlaceholderTag(text, builder, placeholders, tagName, tagsMatch, match, i);
            }
            else if (TemplateRegex.IsArrayTag(tag))
            {
                i = AppendArrayTag(text, builder, conditions, placeholders, arrays, tagsMatch, match, tagName, i);
            }
            else if (TemplateRegex.IsConditionTag(tag))
            {
                i = AppendConditionTag(text, builder, conditions, placeholders, arrays, tagsMatch, match, tagName, i);
            }
        }

        AppendTextAfterLastTag(text, builder, tagsMatch);
    }

    private void AppendTextBeforeFirstTag(string text, StringBuilder builder, MatchCollection tagsMatch)
    {
        var firstTag = tagsMatch[0];
        builder.Append(text, 0, firstTag.Index);
    }

    private void AppendPlaceholderTag(string text, StringBuilder builder,
        Dictionary<string, string> placeholders, string tagName, MatchCollection tagsMatch, Match match, int i)
    {
        builder.Append(placeholders[tagName]);

        if (i < tagsMatch.Count - 1)
        {
            var currentTagEnd = match.Index + match.Length;
            var nextTagStart = tagsMatch[i + 1].Index;
            builder.Append(text, currentTagEnd, nextTagStart - currentTagEnd);
        }
    }

    private int AppendArrayTag(string text, StringBuilder builder, List<string> conditions,
        Dictionary<string, string> placeholders, List<ArrayNode> arrays, MatchCollection tagsMatch,
        Match match, string tagName, int i)
    {
        int tagEndIndex = IsArrayEndTag(tagsMatch, tagName, i + 1);
        
        var startTextIndex = match.Index + match.Length;
        var textEndIndex = tagsMatch[tagEndIndex].Index;

        var formattingFragment = text.Substring(startTextIndex, textEndIndex - startTextIndex);
        
        ArrayNode arrayNode = GetArrayConfiguration(arrays, tagName);
        for (int j = 0; j < arrayNode.Members.Count; j++)
        {
            var member = arrayNode.Members[j];
            var arrayMemberConditions = CombineConditionsFromMemberArray(conditions, arrayNode, member);
            var arrayMemberPlaceholders = CombinePlaceholdersFromMemberArray(placeholders, arrayNode, member);
            Format(formattingFragment, builder, arrayMemberConditions, arrayMemberPlaceholders, member.Arrays);
        }

        return tagEndIndex + 1;
    }

    private int AppendConditionTag(string text, StringBuilder builder, List<string> conditions,
        Dictionary<string, string> placeholders, List<ArrayNode> arrays, MatchCollection tagsMatch,
        Match match, string tagName, int i)
    {
        bool isEnabled = conditions.Contains(tagName);
        int tagEndIndex = FindConditionEndTagIndex(tagsMatch, tagName, i + 1);
        var startTextIndex = match.Index + match.Length;
        var textEndIndex = tagsMatch[tagEndIndex].Index;

        if (isEnabled)
        {
            var formattingFragment = text.Substring(startTextIndex, textEndIndex - startTextIndex);
            Format(formattingFragment, builder, conditions, placeholders, arrays);
        }

        return tagEndIndex + 1;
    }

    private void AppendTextAfterLastTag(string text, StringBuilder builder, MatchCollection tagsMatch)
    {
        var lastTag = tagsMatch[tagsMatch.Count - 1];
        var startPosition = lastTag.Index + lastTag.Length;
        builder.Append(text, startPosition, text.Length - startPosition);
    }

    private ArrayNode GetArrayConfiguration(List<ArrayNode> arrays, string tagName)
    {
        return arrays.First(a => a.Name == tagName.Split('.').Last());
    }

    private Dictionary<string, string> CombinePlaceholdersFromMemberArray(
        Dictionary<string, string> placeholders, ArrayNode arrayNode, ArrayMember member)
    {
        var memberPlaceholders = new Dictionary<string, string>(placeholders);
        var path = new List<string>();

        var node = arrayNode;
        while (node != null)
        {
            path.Add(node.Name);
            node = node.Parent;
        }

        path.Reverse();

        foreach (var placeholder in member.Placeholder)
        {
            var fullName = $"{string.Join('.', path)}.{placeholder.Key}";
            memberPlaceholders[fullName] = placeholder.Value;
        }

        return memberPlaceholders;
    }

    private List<string> CombineConditionsFromMemberArray(List<string> conditions,
        ArrayNode arrayNode, ArrayMember member)
    {
        var memberConditions = new List<string>(conditions);
        var path = new List<string>();

        var tempArrayConfiguration = arrayNode;
        while (tempArrayConfiguration != null)
        {
            path.Add(tempArrayConfiguration.Name);
            tempArrayConfiguration = tempArrayConfiguration.Parent;
        }

        path.Reverse();

        foreach (var condition in member.Definitions)
        {
            var fullName = $"{string.Join('.', path)}.{condition}";
            memberConditions.Add(fullName);
        }

        return memberConditions;
    }

    private int FindConditionEndTagIndex(MatchCollection tagsMatch, string conditionName, int startIndex)
    {
        return tagsMatch.IndexOf(match =>
        {
            string tag = match.Groups[0].Value;
            string tagName = match.Groups[1].Value;
            return TemplateRegex.IsConditionTag(tag) && TemplateRegex.IsClosingTagName(tagName) &&
                   tagName.Substring(1) == conditionName;
        }, startIndex);
    }

    private int IsArrayEndTag(MatchCollection tagsMatch, string arrayName, int startIndex)
    {
        return tagsMatch.IndexOf(match =>
        {
            string tag = match.Groups[0].Value;
            string tagName = match.Groups[1].Value;
            return TemplateRegex.IsArrayTag(tag) && TemplateRegex.IsClosingTagName(tagName) &&
                   tagName.Substring(1) == arrayName;
        }, startIndex);
    }
}