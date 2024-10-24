namespace Re.Template;

public class TemplateFormatArgs
{
    public List<string> Definitions { get; init; } = new();
    public Dictionary<string, string> Placeholder { get; init; } = new();
    public List<ArrayNode> Arrays { get; init; } = new();
}

public class ArrayNode
{
    public string Name { get; init; }
    public ArrayMember? Parent { get; init; }
    public List<ArrayMember> Members { get; init; } = new();
    
    public ArrayNode(string name, ArrayMember? parent)
    {
        Name = name;
        Parent = parent;
    }
}

public class ArrayMember
{
    public ArrayNode Node { get; init; }
    public Dictionary<string, string> Placeholder { get; init; } = new();
    public List<string> Definitions { get; init; } = new();
    public List<ArrayNode> Arrays { get; init; } = new();

    public ArrayMember(ArrayNode node) { Node = node; }
}