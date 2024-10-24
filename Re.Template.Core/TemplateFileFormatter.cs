using System.Collections.Generic;
using System.Text.RegularExpressions;
using Re.Template.Utils;

namespace Re.Template;

public class TemplateFileFormatter
{
    private readonly TemplateTextFormatter _textFormatter;

    public TemplateFileFormatter()
    {
        _textFormatter = new TemplateTextFormatter();
    }

    public List<ArtifactItem> Format(TemplateItem file, TemplateFormatArgs args)
    {
        var artifacts = new List<ArtifactItem>();
        var initialTemplates = new List<FileTemplate>
        {
            new(file.Name, file.Text, args.Placeholder, args.Definitions, args.Arrays)
        };

        var expandedTemplates = ExpandFileTemplates(initialTemplates);

        foreach (var template in expandedTemplates)
        {
            var formattedFileName = _textFormatter.Format(template.FileNameTemplate, new TemplateFormatArgs
            {
                Definitions = template.Conditions,
                Placeholder = template.Placeholders,
                Arrays = template.Arrays
            });

            var formattedContent = _textFormatter.Format(template.ContentTemplate, new TemplateFormatArgs
            {
                Definitions = template.Conditions,
                Placeholder = template.Placeholders,
                Arrays = template.Arrays
            });

            artifacts.Add(new ArtifactItem(formattedFileName, formattedContent));
        }

        var artifactNames = new HashSet<string>();
        foreach (var artifact in artifacts)
        {
            if (artifactNames.Contains(artifact.Name))
                Console.WriteLine($"Collision occured on filename: {artifact.Name}");
            else
                artifactNames.Add(artifact.Name);
        }

        return artifacts;
    }

    private List<FileTemplate> ExpandFileTemplates(List<FileTemplate> templates)
    {
        var result = new List<FileTemplate>();

        foreach (var template in templates)
        {
            var tagsMatch = TemplateRegex.AnyTagRegex.Matches(template.FileNameTemplate);
            bool foundArray = false;

            for (int i = 0; i < tagsMatch.Count; i++)
            {
                var match = tagsMatch[i];
                string tag = match.Groups[0].Value;

                if (TemplateRegex.IsArrayTag(tag))
                {
                    foundArray = true;
                    string arrayTagName = match.Groups[1].Value;

                    int tagEndIndex = TemplateFormatterHelper.IndexOfArrayClosingTag(tagsMatch, arrayTagName, i + 1);
                    var endMatch = tagsMatch[tagEndIndex];

                    var innerTemplate = GetTextBetweenTags(template.FileNameTemplate, match, endMatch);

                    ArrayNode arrayNode = TemplateFormatterHelper.GetArrayConfiguration(template.Arrays, arrayTagName);

                    foreach (var member in arrayNode.Members)
                    {
                        var memberConditions = TemplateFormatterHelper.CombineConditionsFromMemberArray(template.Conditions, member);
                        var memberPlaceholders = TemplateFormatterHelper.CombinePlaceholdersFromMemberArray(template.Placeholders, member);

                        var newFileNameTemplate = template.FileNameTemplate.Substring(0, match.Index)
                            + innerTemplate
                            + template.FileNameTemplate.Substring(endMatch.Index + endMatch.Length);

                        var newContentTemplate = template.ContentTemplate;

                        var newTemplate = new FileTemplate(newFileNameTemplate, newContentTemplate, memberPlaceholders, memberConditions, member.Arrays);

                        result.AddRange(ExpandFileTemplates(new List<FileTemplate> { newTemplate }));
                    }

                    break;
                }
            }

            if (!foundArray)
            {
                result.Add(template);
            }
        }

        return result;
    }

    private string GetTextBetweenTags(string text, Match startTag, Match endTag)
    {
        int startIndex = TemplateFormatterHelper.IndexOfTagEnd(startTag, text);
        int endIndex = TemplateFormatterHelper.IndexOfTagStart(endTag, text);
        return text.Substring(startIndex, endIndex - startIndex);
    }
}

internal class FileTemplate
{
    public string FileNameTemplate { get; }
    public string ContentTemplate { get; }
    public Dictionary<string, string> Placeholders { get; }
    public List<string> Conditions { get; }
    public List<ArrayNode> Arrays { get; }

    public FileTemplate(string fileNameTemplate, string contentTemplate, Dictionary<string, string> placeholders, List<string> conditions, List<ArrayNode> arrays)
    {
        FileNameTemplate = fileNameTemplate;
        ContentTemplate = contentTemplate;
        Placeholders = placeholders;
        Conditions = conditions;
        Arrays = arrays;
    }
}