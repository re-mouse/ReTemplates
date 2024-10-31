using ReDI;

namespace Re.Templates;

public class TemplateFormatArgsFactory
{
    [Inject] private TemplateMetadataFactory _metadataFactory;
    [Inject] private ITemplateArgsGetter _argsGetter;
    
    public TemplateFormatArgs Create(Template template)
    {
        _argsGetter.SetConfiguration(template.Configuration);
        var metadata = _metadataFactory.CreateFromFolder(template.Root);
        
        var args = new TemplateFormatArgs();
        foreach (var condition in metadata.Definitions)
        {
            if (_argsGetter.GetCondition(condition, null))
                args.Definitions.Add(condition);
        }

        foreach (var placeholderKey in metadata.Placeholder)
        {
            string value = _argsGetter.GetPlaceholder(placeholderKey, null);
            args.Placeholder[placeholderKey] = value;
        }

        foreach (var array in metadata.Arrays)
        {
            var arrayConfiguration = GetArrayConfiguration(array, null);
            args.Arrays.Add(arrayConfiguration);
        }
        
        return args;
    }
    
    private ArrayNode GetArrayConfiguration(TemplateArrayMetadata array, ArrayMember? parent)
    {
        var arrayConfiguration = new ArrayNode(array.Name, parent);

        var memberCount = _argsGetter.GetArrayMemberCount(array.Name, parent);
        for (int i = 0; i < memberCount; i++)
        {
            var arrayMember = new ArrayMember(arrayConfiguration);
            arrayConfiguration.Members.Add(arrayMember);
            
            foreach (var placeholder in array.Placeholder)
            {
                arrayMember.Placeholder[placeholder] = _argsGetter.GetPlaceholder(placeholder, arrayMember);
            }
            
            foreach (var condition in array.Definitions)
            {
                if (_argsGetter.GetCondition(condition, arrayMember))
                    arrayMember.Definitions.Add(condition);
            }

            foreach (var includedArray in array.Arrays)
            {
                arrayMember.Arrays.Add(GetArrayConfiguration(includedArray, arrayMember));
            }
        }
        
        return arrayConfiguration;
    }
}