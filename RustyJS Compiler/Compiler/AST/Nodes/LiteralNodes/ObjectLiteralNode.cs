internal class ObjectLiteralNode : ExpressionNode {
    public override NodeType Kind { get; } = NodeType.NullLiteral;
    public List<RustyProperty> Properties { get; }
    public IdentifierNode Extends { get; }
    public string Name { get; }

    public ObjectLiteralNode(List<RustyProperty> properties, string name, IdentifierNode? extends = null) {
        Properties = properties;
        Extends = extends;
        Name = name;
    }

    public override string ToString() {
        string objectBase = $"class {Name}";
        if (Extends != null) {
            objectBase += $" extends {Extends.Text}";
        }
        return $"{objectBase}{{}}";
    }
}
