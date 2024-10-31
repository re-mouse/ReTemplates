namespace Re.Templates;

public interface ITemplateArgsGetter
{
    public void SetConfiguration(TemplateConfiguration configuration);
    public bool GetCondition(string name, ArrayMember? arrayMember);
    public string GetPlaceholder(string name, ArrayMember? arrayMember);
    public int GetArrayMemberCount(string name, ArrayMember? arrayMember);
}