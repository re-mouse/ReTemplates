namespace ReTemplate;

public class TemplateFormatArgsFactory
{
    private readonly Func<string, ArrayMember?, bool> _conditionFunc;
    private readonly Func<string, ArrayMember?, string> _placeholderFunc;
    private readonly Func<string, ArrayNode?, int> _arrayCountFunc;

    public TemplateFormatArgsFactory(Func<string, ArrayMember?, bool> conditionFunc, Func<string, ArrayMember?, string> placeholderFunc,
        Func<string, ArrayNode?, int> arrayCountFunc)
    {
        _conditionFunc = conditionFunc;
        _placeholderFunc = placeholderFunc;
        _arrayCountFunc = arrayCountFunc;
    }
    
    public TemplateFormatArgs Create(TemplateMetadata metadata)
    {
        var configuration = new TemplateFormatArgs();
        foreach (var condition in metadata.Definitions)
        {
            if (_conditionFunc(condition, null))
                configuration.Definitions.Add(condition);
        }

        foreach (var placeholderKey in metadata.Placeholder)
        {
            string value = _placeholderFunc(placeholderKey, null);
            configuration.Placeholder[placeholderKey] = value;
        }

        foreach (var array in metadata.Arrays)
        {
            var arrayConfiguration = GetArrayConfiguration(array, null);
            configuration.Arrays.Add(arrayConfiguration);
        }
        
        return configuration;
    }
    
    private ArrayNode GetArrayConfiguration(TemplateArrayMetadata array, ArrayNode? source)
    {
        var arrayConfiguration = new ArrayNode(array.Name, source);
        
        for (int i = 0; i < _arrayCountFunc(array.Name, source); i++)
        {
            var arrayMember = new ArrayMember(arrayConfiguration);
            foreach (var placeholder in array.Placeholder)
            {
                var placeholderKey = $"{placeholder}[{i}]";
                arrayMember.Placeholder[placeholder] = _placeholderFunc(placeholderKey, arrayMember);
            }
            
            foreach (var condition in array.Definitions)
            {
                var conditionKey = $"{condition}[{i}]";
                if (_conditionFunc(condition, arrayMember))
                    arrayMember.Definitions.Add(conditionKey);
            }

            foreach (var includedArray in array.Arrays)
            {
                arrayMember.Arrays.Add(GetArrayConfiguration(includedArray, arrayConfiguration));
            }
            
            arrayConfiguration.Members.Add(arrayMember);
        }
        
        return arrayConfiguration;
    }
}