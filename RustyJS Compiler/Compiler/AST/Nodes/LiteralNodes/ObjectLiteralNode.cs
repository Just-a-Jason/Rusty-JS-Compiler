internal class ObjectLiteralNode : ExpressionNode {
    public List<RustyProperty> Properties { get; }
    public override NodeType Kind { get; } = NodeType.NullLiteral;
    public string Name { get; }

    public ObjectLiteralNode(List<RustyProperty> properties, string name) {
        Properties = properties;
        Name = name;
    }

    public override string ToString() {
        return $"class {Name} {{ }};";
    }
}
