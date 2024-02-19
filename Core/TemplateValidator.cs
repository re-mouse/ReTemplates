using System.Text;
using System.Text.RegularExpressions;

namespace ReTemplate;

public class TemplateValidator
{
    internal class TemplateValidationData
    {
        public Stack<string> OpenedArrayTagNames { get; } = new Stack<string>();
        public Stack<string> OpenedConditionTagNames { get; } = new Stack<string>();
        public Stack<string> OpenedTagNames { get; } = new Stack<string>();
    }

    public void Validate(TemplateDirectory templateDirectory)
    {
        ValidateDirectory(templateDirectory);
    }
    
    public void Validate(TemplateFile templateFile)
    {
        ValidateFile(templateFile);
    }
    
    private void ValidateDirectory(TemplateDirectory templateDirectory)
    {
        foreach (var directory in templateDirectory.Directories)
        {
            ValidateDirectory(directory);
        }
        
        foreach (var file in templateDirectory.Files)
        {
            ValidateFile(file);
        }
    }
    
    private void ValidateFile(TemplateFile file)
    {
        try
        {
            Validate(file.Text);
        }
        catch (Exception exception)
        {
            throw new FormatException($"Error occured while validated: {file.Name}. {exception.Message}");
        }
    }

    private void Validate(string text)
    {
        var data = new TemplateValidationData();
        var matches = TemplateRegex.AnyTagRegex.Matches(text);

        foreach (Match match in matches)
        {
            string tag = match.Groups[0].Value;
            string tagName = match.Groups[1].Value;
            ValidateTag(tag, tagName, data);
        }

        if (data.OpenedArrayTagNames.Count != 0)
            throw new FormatException($"Not all arrays was closed. Missed {data.OpenedArrayTagNames.Peek()}");
        
        if (data.OpenedConditionTagNames.Count != 0)
            throw new FormatException($"Not all conditinos was closed. Missed {data.OpenedConditionTagNames.Peek()}");
    }

    private void ValidateTag(string tag, string tagName, TemplateValidationData data)
    {
        if (TemplateRegex.IsConditionTag(tag))
        {
            ValidateConditionTagName(tagName, data);
        }
        else if (TemplateRegex.IsArrayTag(tag))
        {
            ValidateArrayTagName(tagName, data);
        }
        else if (TemplateRegex.IsPlaceholderTag(tag))
        {
            ValidatePlaceholderTagName(tagName, data);
        }
    }

    private void ValidateConditionTagName(string tagName, TemplateValidationData data)
    {
        if (TemplateRegex.IsClosingTagName(tagName))
        {
            if (data.OpenedConditionTagNames.Count == 0)
                throw new FormatException($"Found closing condition tag {tagName}, but not found opened conditions");
            
            if (data.OpenedConditionTagNames.Peek() != tagName.Substring(1))
                throw new FormatException($"Tried to close condition tag {tagName}, while not all child condition tags closed. {data.OpenedConditionTagNames.Peek()} must be closed first");
            
            if (data.OpenedTagNames.Peek() != tagName.Substring(1))
                throw new FormatException($"Tried to close condition tag {tagName}, but {data.OpenedTagNames.Peek()} must be closed first");

            data.OpenedTagNames.Pop();
            data.OpenedConditionTagNames.Pop();
        }
        else
        {
            data.OpenedTagNames.Push(tagName);
            data.OpenedConditionTagNames.Push(tagName);
            ValidateArrayMemberTagName(tagName, data);
        }
    }
    
    private void ValidateArrayTagName(string tagName, TemplateValidationData data)
    {
        if (TemplateRegex.IsClosingTagName(tagName))
        {
            if (data.OpenedArrayTagNames.Count == 0)
                throw new FormatException($"Found closing array tag {tagName}, but not found opened arrays");
            
            if (data.OpenedArrayTagNames.Peek() != tagName.Substring(1))
                throw new FormatException($"Tried to close root array {tagName}, while not all child array tags closed. {data.OpenedArrayTagNames.Peek()} must be closed first");

            if (data.OpenedTagNames.Peek() != tagName.Substring(1))
                throw new FormatException($"Tried to close condition tag {tagName}, but {data.OpenedTagNames.Peek()} must be closed first");

            data.OpenedTagNames.Pop();
            data.OpenedArrayTagNames.Pop();
        }
        else
        {
            if (data.OpenedArrayTagNames.Contains(tagName))
                throw new FormatException("Tried to open array inside same one, maybe you missed closing tag?");
            
            data.OpenedTagNames.Push(tagName);
            data.OpenedArrayTagNames.Push(tagName);
            ValidateArrayMemberTagName(tagName, data);
        }
    }
    
    private void ValidatePlaceholderTagName(string tagName, TemplateValidationData data)
    {
        if (TemplateRegex.IsClosingTagName(tagName))
            throw new FormatException($"Placeholder {tagName} cannot be closing");
        
        ValidateArrayMemberTagName(tagName, data);
    }

    private void ValidateArrayMemberTagName(string tagName, TemplateValidationData data)
    {
        string[] groups = tagName.Split('.');
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < groups.Length - 1; i++)
        {
            stringBuilder.Clear();
            for (int j = 0; j <= i; j++)
            {
                if (j > 0)
                    stringBuilder.Append('.');
                stringBuilder.Append(groups[j]);
            }
            var arrayMember = stringBuilder.ToString();
            
            if (!data.OpenedArrayTagNames.Contains(arrayMember))
                throw new FormatException($"Not found array on what {tagName} refers");
        }
    }
}