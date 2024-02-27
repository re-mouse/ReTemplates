namespace ReTemplate.CLI;

public class CliTemplateArgsGetter : ITemplateArgsGetter
{
    private TemplateConfiguration _currentConfiguration;

    public void SetConfiguration(TemplateConfiguration configuration)
    {
        _currentConfiguration = configuration;
    }

    public bool GetCondition(string name, ArrayMember? arrayMember)
    {
        var fullName = GetFullName(name, arrayMember);
        var positiveOptions = new[] { "Y", "Yes", "True" }; 
        var negativeOptions = new[] { "N", "No", "False" };
        
        var options = positiveOptions.Concat(negativeOptions).ToArray();
        var answer = GetAnswer(fullName,
            $"Enable condition for {fullName}?\n[Y]es/[N]o: ", 
            $"Invalid answer. Select from {string.Join(',', options)}\nY[es]/N[no]: ", 
            answer => options.Contains(answer, StringComparer.CurrentCultureIgnoreCase));
        Console.WriteLine();
        
        return positiveOptions.Contains(answer, StringComparer.CurrentCultureIgnoreCase);
    }

    public string GetPlaceholder(string name, ArrayMember? arrayMember)
    {
        var fullName = GetFullName(name, arrayMember);
        var answer = GetAnswer(fullName, 
            $"Enter value for placeholder {fullName}\n{name}: ", 
            "", 
            answer => true);
        Console.WriteLine();
        
        return answer;
    }

    public int GetArrayMemberCount(string name, ArrayMember? arrayMember)
    {
        var fullName = GetFullName(name, arrayMember);
        var answer = GetAnswer(fullName, 
            $"Enter size for array {fullName}\nSize of {name}: ", 
            $"Invalid. Should be number\nSize of {name}: ", 
            answer => int.TryParse(answer, out _));
        Console.WriteLine();
        
        int.TryParse(answer, out var number);
        return number;
    }

    private string GetValueFromConfig(string fullName)
    {
        return _currentConfiguration.DefaultValues[fullName];
    }

    private bool IsExistInConfig(string fullName)
    {
        return _currentConfiguration?.DefaultValues.ContainsKey(fullName) ?? false;
    }

    private string GetFullName(string name, ArrayMember? arrayMember)
    {
        var baseName = arrayMember != null ? GetPrefix(arrayMember) : "";
        return $"{baseName}{name}";
    }

    private string GetPrefix(ArrayMember member)
    {
        int index = member.Node.Members.IndexOf(member);
        var arrayName = member.Node.Name;
        
        if (member.Node.Parent != null)
            return $"{GetPrefix(member.Node.Parent)}{arrayName}[{index}].";
        
        return $"{arrayName}[{index}].";
    }

    private string GetAnswer(string fullName, string prompt, string wrongAnswer, Func<string?, bool> validateFunc)
    {
        Console.Write(prompt, fullName);
        bool defaultValueUsed = false;
        bool defaultValueExist = IsExistInConfig(fullName);
        while (true)
        {
            string? result;
            if (defaultValueExist && !defaultValueUsed)
            {
                result = GetValueFromConfig(fullName);
                Console.WriteLine($"Found default value from config, value: {result}");
                defaultValueUsed = true;
            }
            else
            {
                result = Console.ReadLine();
            }
            
            if (!validateFunc(result))
            {
                Console.Write(wrongAnswer, fullName);
            }
            else
            {
                return result;
            }
        }
    }
}