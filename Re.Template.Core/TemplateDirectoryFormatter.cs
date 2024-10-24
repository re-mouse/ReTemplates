using System.Text.RegularExpressions;
using Re.Template.Utils;
using ReDI;

namespace Re.Template
{
    public class TemplateDirectoryFormatter
    {
        [Inject] private TemplateTextFormatter _textFormatter;
        [Inject] private TemplateFileFormatter _fileFormatter;

        public ArtifactFolder Format(TemplateFolder folder, TemplateFormatArgs args)
        {
            var rootArtifactFolder = new ArtifactFolder("");

            var initialTemplates = new List<FolderTemplate>
            {
                new(folder.Name, folder.Items, folder.Folders, args.Placeholder, args.Definitions, args.Arrays)
            };

            var expandedTemplates = ExpandFolderTemplates(initialTemplates);

            foreach (var folderTemplate in expandedTemplates)
            {
                var formattedFolderName = _textFormatter.Format(folderTemplate.FolderNameTemplate, new TemplateFormatArgs
                {
                    Definitions = folderTemplate.Conditions,
                    Placeholder = folderTemplate.Placeholders,
                    Arrays = folderTemplate.Arrays
                });
                
                if (string.IsNullOrEmpty(formattedFolderName))
                    continue;

                var artifactFolder = new ArtifactFolder(formattedFolderName);

                foreach (var item in folderTemplate.Items)
                {
                    var artifacts = _fileFormatter.Format(item, new TemplateFormatArgs
                    {
                        Definitions = folderTemplate.Conditions,
                        Placeholder = folderTemplate.Placeholders,
                        Arrays = folderTemplate.Arrays
                    });

                    artifactFolder.Items.AddRange(artifacts);
                }

                foreach (var subFolder in folderTemplate.Folders)
                {
                    var subArtifactFolder = Format(subFolder, new TemplateFormatArgs
                    {
                        Definitions = folderTemplate.Conditions,
                        Placeholder = folderTemplate.Placeholders,
                        Arrays = folderTemplate.Arrays
                    });

                    if (!string.IsNullOrEmpty(subArtifactFolder.Name))
                    {
                        artifactFolder.Folders.Add(subArtifactFolder);
                    }
                    else
                    {
                        artifactFolder.Folders.AddRange(subArtifactFolder.Folders);
                        artifactFolder.Items.AddRange(subArtifactFolder.Items);
                    }
                }

                rootArtifactFolder.Folders.Add(artifactFolder);
            }

            return rootArtifactFolder;
        }

        private List<FolderTemplate> ExpandFolderTemplates(List<FolderTemplate> templates)
        {
            var result = new List<FolderTemplate>();

            foreach (var template in templates)
            {
                var tagsMatch = TemplateRegex.AnyTagRegex.Matches(template.FolderNameTemplate);
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

                        var innerTemplate = GetTextBetweenTags(template.FolderNameTemplate, match, endMatch);

                        ArrayNode arrayNode = TemplateFormatterHelper.GetArrayConfiguration(template.Arrays, arrayTagName);

                        foreach (var member in arrayNode.Members)
                        {
                            var memberConditions = TemplateFormatterHelper.CombineConditionsFromMemberArray(template.Conditions, member);
                            var memberPlaceholders = TemplateFormatterHelper.CombinePlaceholdersFromMemberArray(template.Placeholders, member);

                            var newFolderNameTemplate = template.FolderNameTemplate.Substring(0, match.Index)
                                                        + innerTemplate
                                                        + template.FolderNameTemplate.Substring(endMatch.Index + endMatch.Length);

                            var newTemplate = new FolderTemplate(newFolderNameTemplate, template.Items, template.Folders,
                                                                 memberPlaceholders, memberConditions, member.Arrays);

                            result.AddRange(ExpandFolderTemplates(new List<FolderTemplate> { newTemplate }));
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

    internal class FolderTemplate
    {
        public string FolderNameTemplate { get; }
        public List<TemplateItem> Items { get; }
        public List<TemplateFolder> Folders { get; }
        public Dictionary<string, string> Placeholders { get; }
        public List<string> Conditions { get; }
        public List<ArrayNode> Arrays { get; }

        public FolderTemplate(string folderNameTemplate, List<TemplateItem> items, List<TemplateFolder> folders,
                              Dictionary<string, string> placeholders, List<string> conditions, List<ArrayNode> arrays)
        {
            FolderNameTemplate = folderNameTemplate;
            Items = items;
            Folders = folders;
            Placeholders = placeholders;
            Conditions = conditions;
            Arrays = arrays;
        }
    }
}