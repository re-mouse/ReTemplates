using System.Text.RegularExpressions;
using Re.Template;
using Re.Template.Utils;

public class TemplateMetadataFactory
{
    public TemplateMetadata CreateFromFolder(TemplateFolder folder)
    {
        TemplateMetadata metadata = new TemplateMetadata();
        FillMetadataFromFolder(folder, metadata);
        return metadata;
    }
    
    public TemplateMetadata CreateFromItem(TemplateItem item)
    {
        TemplateMetadata metadata = new TemplateMetadata();
        FillMetadataFromItem(item, metadata);
        return metadata;
    }
    
    private void FillMetadataFromFolder(TemplateFolder templateFolder, TemplateMetadata metadata)
    {
        foreach (var file in templateFolder.Items)
        {
            FillMetadataFromItem(file, metadata);
        }

        foreach (var templateSubdirectory in templateFolder.Folders)
        {
            FillMetadataFromFolder(templateSubdirectory, metadata);
        }
    }

    private void FillMetadataFromItem(TemplateItem item, TemplateMetadata metadata)
    {
        var matches = TemplateRegex.AnyTagRegex.Matches(item.Text);

        foreach (Match match in matches)
        {
            string tag = match.Groups[0].Value;
            string tagName = match.Groups[1].Value;
            FillMetadataFromTag(tag, tagName, metadata);
        }
    }

    private void FillMetadataFromTag(string tag, string tagName, TemplateMetadata data)
    {
        if (TemplateRegex.IsConditionTag(tag) && !data.Definitions.Contains(tagName) && !TemplateRegex.IsClosingTagName(tagName))
        {
            FillConditionMetadata(tagName, data);
        }
        else if (TemplateRegex.IsArrayTag(tag) && !TemplateRegex.IsClosingTagName(tagName))
        {
            FillArrayMetadata(tagName, data);
        }
        else if (TemplateRegex.IsPlaceholderTag(tag))
        {
            FillPlaceholderMetadata(tagName, data);
        }
    }

    private void FillConditionMetadata(string tagName, TemplateMetadata data)
    {
        var path = tagName.Split('.');
        var condition = path[path.Length - 1];
        var arrays = data.Arrays;
        List<string> conditionList = data.Definitions;
        for (int i = 0; i < path.Length - 1; i++)
        {
            var currentArray = arrays.First(array => array.Name == path[i]);
            arrays = currentArray.Arrays;
            conditionList = currentArray.Definitions;
        }
            
        if (!conditionList.Contains(condition))
            conditionList.Add(condition);
    }

    private void FillArrayMetadata(string tagName, TemplateMetadata data)
    {
        var path = tagName.Split('.');
        var currentArraysList = data.Arrays;
        for (int i = 0; i < path.Length; i++)
        {
            if (!currentArraysList.Exists(array => array.Name == path[i]))
            {
                currentArraysList.Add(new TemplateArrayMetadata(path[i]));
            }

            currentArraysList = currentArraysList.First(array => array.Name == path[i]).Arrays;
        }
    }

    private void FillPlaceholderMetadata(string tagName, TemplateMetadata data)
    {
        var path = tagName.Split('.');
        var placeholder = path[path.Length - 1];
        var arrays = data.Arrays;
        List<string> placeholdersList = data.Placeholder;
        for (int i = 0; i < path.Length - 1; i++)
        {
            var currentArray = arrays.First(array => array.Name == path[i]);
            arrays = currentArray.Arrays;
            placeholdersList = currentArray.Placeholder;
        }
            
        if (!placeholdersList.Contains(placeholder))
            placeholdersList.Add(placeholder);
    }
}