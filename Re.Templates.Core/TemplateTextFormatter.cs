using System.Text;
using System.Text.RegularExpressions;
using Re.Templates.Utils;

namespace Re.Templates;

public class TemplateTextFormatter
{
    private StringBuilder _builder;
    
    public string Format(string text, TemplateFormatArgs args)
    {
        _builder = new StringBuilder();
        Format(text, args.Definitions, args.Placeholder, args.Arrays);
        return _builder.ToString();
    }

    private void Format(string text, List<string> conditions,
        Dictionary<string, string> placeholders, List<ArrayNode> arrays)
    {
        var tagsMatch = TemplateRegex.AnyTagRegex.Matches(text);
        if (tagsMatch.Count == 0)
        {
            _builder.Append(text);
            return;
        }

        _builder.Append(GetTextBetweenTags(text, null, tagsMatch[0]));
        for (int i = 0; i < tagsMatch.Count; i++)
        {
            var match = tagsMatch[i];
            string tag = match.Groups[0].Value;

            if (TemplateRegex.IsPlaceholderTag(tag))
            {
                AppendPlaceholder(text, placeholders, tagsMatch, i);
            }
            else if (TemplateRegex.IsArrayTag(tag))
            {
                i = AppendArrayTag(text, conditions, placeholders, arrays, tagsMatch, i);
            }
            else if (TemplateRegex.IsConditionTag(tag))
            {
                i = AppendConditionTag(text, conditions, placeholders, arrays, tagsMatch, i);
            }
        }
    }

    private string GetTextBetweenTags(string text, Match? firstTag, Match? secondTag)
    {
        var start = firstTag != null ? TemplateFormatterHelper.IndexOfTagEnd(firstTag, text) : 0;
        var end = secondTag != null ? TemplateFormatterHelper.IndexOfTagStart(secondTag, text) : text.Length;
        return text.Substring(start, end - start);
    }

    private void AppendPlaceholder(string text, Dictionary<string, string> placeholders, MatchCollection tagsMatch, int tagIndex)
    {
        var match = tagsMatch[tagIndex];
        string tagName = match.Groups[1].Value;
        _builder.Append(placeholders[tagName]);

        var nextMatch = tagIndex + 1 < tagsMatch.Count ? tagsMatch[tagIndex + 1] : null;
        _builder.Append(GetTextBetweenTags(text, match, nextMatch));
    }

    private int AppendArrayTag(string text, List<string> conditions, Dictionary<string, string> placeholders, List<ArrayNode> arrays, MatchCollection tagsMatch, int tagIndex)
    {
        var match = tagsMatch[tagIndex];
        string tagName = match.Groups[1].Value;
        
        int tagEndIndex = TemplateFormatterHelper.IndexOfArrayClosingTag(tagsMatch, tagName, tagIndex + 1);
        var endMatch = tagsMatch[tagEndIndex];

        var formattingFragment = GetTextBetweenTags(text, match, endMatch);

        ArrayNode arrayNode = TemplateFormatterHelper.GetArrayConfiguration(arrays, tagName);
        for (int j = 0; j < arrayNode.Members.Count; j++)
        {
            var member = arrayNode.Members[j];
            var arrayMemberConditions = TemplateFormatterHelper.CombineConditionsFromMemberArray(conditions, member);
            var arrayMemberPlaceholders = TemplateFormatterHelper.CombinePlaceholdersFromMemberArray(placeholders, member);
            Format(formattingFragment, arrayMemberConditions, arrayMemberPlaceholders, member.Arrays);
        }
        
        var endNextMatch = tagEndIndex + 1 < tagsMatch.Count ? tagsMatch[tagEndIndex + 1] : null;
        _builder.Append(GetTextBetweenTags(text, endMatch, endNextMatch));

        return tagEndIndex;
    }

    private int AppendConditionTag(string text, List<string> conditions,
        Dictionary<string, string> placeholders, List<ArrayNode> arrays, MatchCollection tagsMatch, int tagIndex)
    {
        var match = tagsMatch[tagIndex];
        string tagName = match.Groups[1].Value;
        
        bool isEnabled = conditions.Contains(tagName);
        
        int tagEndIndex = TemplateFormatterHelper.IndexOfConditionClosingTag(tagsMatch, tagName, tagIndex + 1);
        var endMatch = tagsMatch[tagEndIndex];
        
        if (isEnabled)
        {
            var formattingFragment = GetTextBetweenTags(text, match, endMatch);
            Format(formattingFragment, conditions, placeholders, arrays);
        }

        var endNextMatch = tagEndIndex + 1 < tagsMatch.Count ? tagsMatch[tagEndIndex + 1] : null;
        _builder.Append(GetTextBetweenTags(text, endMatch, endNextMatch));

        return tagEndIndex;
    }
}