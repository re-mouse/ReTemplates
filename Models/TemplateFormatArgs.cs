namespace ReTemplate;

public class TemplateFormatArgs
{
    public List<string> Definitions { get; } = new List<string>();
    public Dictionary<string, string> Placeholder { get; } = new Dictionary<string, string>();
    public List<ArrayNode> Arrays { get; } = new List<ArrayNode>();
}

public class ArrayNode
{
    public string Name { get; }
    public ArrayNode? Parent { get; }
    public List<ArrayMember> Members { get; } = new List<ArrayMember>();
    
    public ArrayNode(string name, ArrayNode? parent)
    {
        Name = name;
        Parent = parent;
    }
}

public class ArrayMember
{
    public ArrayNode Node { get; }
    public Dictionary<string, string> Placeholder { get; } = new Dictionary<string, string>();
    public List<string> Definitions { get; } = new List<string>();
    public List<ArrayNode> Arrays { get; } = new List<ArrayNode>();

    public ArrayMember(ArrayNode node) { Node = node; }
}