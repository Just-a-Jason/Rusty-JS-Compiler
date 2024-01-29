internal class ObjectLiteralNode : ExpressionNode {
    public List<RustyProperty> Properties { get; }
    public override NodeType Kind { get; } = NodeType.NullLiteral;

    public ObjectLiteralNode(List<RustyProperty> properties) {
        Properties = properties;
    }
}
